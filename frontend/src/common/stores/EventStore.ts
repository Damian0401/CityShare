import { makeAutoObservable, runInAction } from "mobx";
import { IEvent, IEventSearchQuery } from "../interfaces";
import agent from "../api/agent";

export default class EventStore {
  selectedEvent: IEvent | null = null;

  constructor() {
    makeAutoObservable(this);
  }

  selectEvent = (event: IEvent | null) => {
    runInAction(() => {
      this.selectedEvent = event;
    });
  };

  clearSelectedEvent = () => {
    runInAction(() => {
      this.selectedEvent = null;
    });
  };

  updateEvent = (event: IEvent) => {
    if (!this.selectedEvent) return;

    runInAction(() => {
      this.selectedEvent = { ...this.selectedEvent, ...event };
    });
  };

  getEvents = (
    query: IEventSearchQuery,
    pageNumber?: number,
    pageSize?: number
  ) => {
    return agent.Event.get(query, pageNumber, pageSize);
  };
}
