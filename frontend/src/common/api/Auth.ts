import { ILoginValues, IRegisterValues, IUser } from "../interfaces";
import { IProfile } from "../interfaces/IProfile";
import { AccessTokenHelper } from "../utils/helpers";
import requests from "../utils/requests";

const Auth = {
  login: (values: ILoginValues) => requests.post<IUser>("/auth/login", values),
  register: (values: IRegisterValues) =>
    requests.post<IUser>("/auth/register", values),
  refresh: (signal?: AbortSignal) => {
    const accessToken = AccessTokenHelper.getAccessToken();
    return requests.post<IUser>("/auth/refresh", { accessToken }, signal);
  },
  confirmEmail: (id: string, token: string, signal: AbortSignal) =>
    requests.post("/auth/confirm-email", { id, token }, signal),
  profile: () => requests.get<IProfile>("/auth/profile"),
};

export default Auth;
