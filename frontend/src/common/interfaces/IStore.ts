import AuthStore from "../stores/AuthStore";
import CommonStore from "../stores/CommonStore";
import EventStore from "../stores/EventStore";
import RequestStore from "../stores/RequestStore";

export interface IStore {
  authStore: AuthStore;
  commonStore: CommonStore;
  eventStore: EventStore;
  requestStore: RequestStore;
}
