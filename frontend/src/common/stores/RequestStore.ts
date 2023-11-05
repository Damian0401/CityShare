import { makeAutoObservable, runInAction } from "mobx";
import { IRequests } from "../interfaces";
import agent from "../api/agent";

export default class RequestStore {
  private loadedRequests: Map<number, IRequests> = new Map<number, IRequests>();

  constructor() {
    makeAutoObservable(this);
  }

  getRequests = async (cityId: number, signal?: AbortSignal) => {
    const requests = this.loadedRequests.get(cityId);

    if (requests) {
      return requests;
    }

    const newRequests = await agent.Requests.getRequestsByCityId(
      cityId,
      signal
    );

    runInAction(() => {
      this.loadedRequests.set(cityId, newRequests);
    });

    return newRequests;
  };
}
