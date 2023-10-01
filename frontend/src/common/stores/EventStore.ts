import { makeAutoObservable, runInAction } from "mobx";
import { IEvent, IEventSearchQuery } from "../interfaces";
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

  loadSelectedEvent = async (id: string) => {
    if (this.selectedEvent && this.selectedEvent.id === id) {
      return;
    }

    const event = await agent.Event.getById(id);

    runInAction(() => {
      this.selectedEvent = event;
    });
  };
}
