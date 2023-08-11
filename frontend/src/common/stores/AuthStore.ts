import { makeAutoObservable, reaction, runInAction } from "mobx";
import { ILoginValues } from "../interfaces";
import agent from "../api/agent";
import { IRegisterValues } from "../interfaces/IRegisterValues";
import { IUser } from "../interfaces/IUser";
import { accessTokenHelper } from "../utils/helpers";
export default class AuthStore {
  private currentUser: IUser | null = null;

  constructor() {
    makeAutoObservable(this);

    reaction(
      () => this.currentUser,
      (currentUser) => {
        if (currentUser) {
          accessTokenHelper.setAccessToken(currentUser.accessToken);
        } else {
          accessTokenHelper.removeAccessToken();
        }
      }
    );
  }

  get user() {
    return this.currentUser;
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

  refresh = async () => {
    const user = await agent.Auth.refresh();
    runInAction(() => {
      this.currentUser = user;
    });
  };

  confirmEmail = async (id: string, token: string, signal: AbortSignal) => {
    await agent.Auth.confirmEmail(id, token, signal);
    runInAction(() => {
      if (!this.currentUser) return;
      this.currentUser.emailConfirmed = true;
    });
  };
}
