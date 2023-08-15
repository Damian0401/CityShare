import { createContext, useContext } from "react";
import AuthStore from "./AuthStore";
import { IStore } from "../interfaces";
import CommonStore from "./CommonStore";

export const store: IStore = {
  authStore: new AuthStore(),
  commonStore: new CommonStore(),
};

export const StoreContext = createContext(store);

export function useStore() {
  return useContext(StoreContext);
}
