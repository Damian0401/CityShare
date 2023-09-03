import AuthStore from "../stores/AuthStore";
import CommonStore from "../stores/CommonStore";

export interface IStore {
  authStore: AuthStore;
  commonStore: CommonStore;
}
