import { makeAutoObservable, runInAction } from "mobx";
import {
  IComment,
  IEvent,
  IEventCreateValues,
  IEventSearchQuery,
} from "../interfaces";
import agent from "../api/agent";
import {
  AccessTokenHelper,
  correctCommentDate,
  getSecret,
  updateLikes,
} from "../utils/helpers";
import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";
import { Environments } from "../enums";
import { CommentHubMethods } from "../enums/CommentHubMethods";
import Constants from "../utils/constants";

export default class EventStore {
  selectedEvent: IEvent | null = null;
  comments: IComment[] = [];
  hubConnection: HubConnection | null = null;

  constructor() {
    makeAutoObservable(this);
  }

  setSelectedEvent = (event: IEvent | null) => {
    runInAction(() => {
      this.selectedEvent = event;
    });
  };

  clearSelectedEvent = () => {
    runInAction(() => {
      this.selectedEvent = null;
    });
  };

  getEventsByQuery = (query: IEventSearchQuery, signal?: AbortSignal) => {
    return agent.Event.get(query, signal);
  };

  loadSelectedEvent = async (id: string, signal?: AbortSignal) => {
    if (this.selectedEvent && this.selectedEvent.id === id) {
      return;
    }

    const event = await agent.Event.getById(id, signal);

    runInAction(() => {
      this.selectedEvent = event;
    });
  };

  createEvent = async (values: IEventCreateValues) => {
    const images = [...(values.images ?? [])];
    delete values.images;

    const id = await agent.Event.create(values);

    for (const image of images) {
      await agent.Event.uploadImage(id, image);
    }

    return id;
  };

  updateLikes = async (id: string) => {
    await agent.Event.updateLikes(id);

    runInAction(() => {
      if (!this.selectedEvent || this.selectedEvent.id !== id) {
        return;
      }

      updateLikes(this.selectedEvent);
    });
  };

  addComment = async (id: string, comment: string) => {
    await agent.Event.addComment(id, comment);
  };

  createHubConnection = async () => {
    if (!this.selectedEvent) return;

    const comments = await agent.Event.getComments(this.selectedEvent.id);

    runInAction(() => {
      this.comments = comments;

      if (!this.selectedEvent) {
        return;
      }

      this.selectedEvent.comments = comments.length;
    });

    this.hubConnection = new HubConnectionBuilder()
      .withUrl(
        getSecret(Environments.BaseUrl) +
          Constants.HubPrefix +
          `/comments?event_id=${this.selectedEvent.id}`,
        {
          accessTokenFactory: () => AccessTokenHelper.getAccessToken() ?? "",
        }
      )
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();

    this.hubConnection.on(CommentHubMethods.AddComment, (comment: IComment) => {
      correctCommentDate(comment);
      runInAction(() => {
        this.comments.push(comment);
      });

      runInAction(() => {
        if (!this.selectedEvent) {
          return;
        }
        this.selectedEvent.comments += 1;
      });
    });

    this.hubConnection.start().catch((error) => console.log(error));
  };

  stopHubConnection = () => {
    this.hubConnection?.stop().catch((error) => console.log(error));
  };
}
