import { makeAutoObservable, runInAction } from "mobx";
import { IEvent, IEventCreateValues, IEventSearchQuery } from "../interfaces";
import agent from "../api/agent";

export default class EventStore {
  selectedEvent: IEvent | null = null;

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
}