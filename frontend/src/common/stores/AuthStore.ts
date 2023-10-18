import { makeAutoObservable, reaction, runInAction } from "mobx";
import { ILoginValues, IUser } from "../interfaces";
import agent from "../api/agent";
import { IRegisterValues } from "../interfaces/IRegisterValues";
import { AccessTokenHelper } from "../utils/helpers";
export default class AuthStore {
  user: IUser | null = null;

  constructor() {
    makeAutoObservable(this);

    reaction(
      () => this.user,
      (user) => {
        if (user) {
          AccessTokenHelper.setAccessToken(user.accessToken);
        } else {
          AccessTokenHelper.removeAccessToken();
        }
      }
    );
  }

  login = async (values: ILoginValues) => {
    const user = await agent.Auth.login(values);
    runInAction(() => {
      this.user = user;
    });
  };

  logout = () => {
    runInAction(() => {
      this.user = null;
    });
  };

  register = async (values: IRegisterValues) => {
    const user = await agent.Auth.register(values);
    runInAction(() => {
      this.user = user;
    });
  };

  refresh = async (signal?: AbortSignal) => {
    const user = await agent.Auth.refresh(signal);
    runInAction(() => {
      this.user = user;
    });
  };

  confirmEmail = async (id: string, token: string, signal: AbortSignal) => {
    await agent.Auth.confirmEmail(id, token, signal);
    runInAction(() => {
      if (!this.user) return;
      this.user.emailConfirmed = true;
    });
  };
}
