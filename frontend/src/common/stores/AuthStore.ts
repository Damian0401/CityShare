import { makeAutoObservable, reaction } from "mobx";
import { StorageKeys } from "../enums";

export default class AuthStore {
  private accessToken: string | null = window.localStorage.getItem(
    StorageKeys.AccessToken
  );

  constructor() {
    makeAutoObservable(this);

    reaction(
      () => this.accessToken,
      (accessToken) => {
        if (accessToken) {
          window.localStorage.setItem(StorageKeys.AccessToken, accessToken);
        } else {
          window.localStorage.removeItem(StorageKeys.AccessToken);
        }
      }
    );
  }

  setToken(token: string) {
    this.accessToken = token;
  }

  removeTokens() {
    this.accessToken = null;
  }
}
