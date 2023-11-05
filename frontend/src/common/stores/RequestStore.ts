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

  acceptRequest = async (id: string) => {
    await agent.Requests.accept(id);
  };

  rejectRequest = async (id: string) => {
    await agent.Requests.reject(id);
  };

  removeRequest = (cityId: number, requestId: string) => {
    const requests = this.loadedRequests.get(cityId);

    if (!requests) return;

    for (const pair of requests.requests) {
      const index = pair[1].findIndex((r) => r.id === requestId);

      if (index === -1) continue;

      runInAction(() => {
        pair[1].splice(index, 1);
      });

      break;
    }
  };
}
