import { createContext, useContext } from "react";
import AuthStore from "./AuthStore";
import { IStore } from "../interfaces";
import CommonStore from "./CommonStore";
import EventStore from "./EventStore";
import RequestStore from "./RequestStore";

export const store: IStore = {
  authStore: new AuthStore(),
  commonStore: new CommonStore(),
  eventStore: new EventStore(),
  requestStore: new RequestStore(),
};

export const StoreContext = createContext(store);

export function useStore() {
  return useContext(StoreContext);
}
