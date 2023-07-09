import { createContext, useContext } from "react";
import AuthStore from "./AuthStore";
import { IStore } from "../interfaces";

export const store: IStore = {
  authStore: new AuthStore(),
};

export const StoreContext = createContext(store);

export function useStore() {
  return useContext(StoreContext);
}
