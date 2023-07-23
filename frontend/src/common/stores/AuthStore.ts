import { makeAutoObservable, reaction, runInAction } from "mobx";
import { StorageKeys } from "../enums";
import { ILoginValues } from "../interfaces";
import agent from "../api/agent";
import { IRegisterValues } from "../interfaces/IRegisterValues";
import { IUser } from "../interfaces/IUser";

export default class AuthStore {
  private currentUser: IUser | null = null;

  constructor() {
    makeAutoObservable(this);

    reaction(
      () => this.currentUser,
      (currentUser) => {
        if (currentUser) {
          window.localStorage.setItem(
            StorageKeys.AccessToken,
            currentUser.accessToken
          );
        } else {
          window.localStorage.removeItem(StorageKeys.AccessToken);
        }
      }
    );
  }

  login = async (values: ILoginValues) => {
    const user = await agent.Auth.login(values);
    runInAction(() => {
      this.currentUser = user;
    });
  };

  logout = () => {
    runInAction(() => {
      this.currentUser = null;
    });
  };

  register = async (values: IRegisterValues) => {
    const user = await agent.Auth.register(values);
    runInAction(() => {
      this.currentUser = user;
    });
  };

  get user() {
    return this.currentUser;
  }
}
