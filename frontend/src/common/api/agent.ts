import axios, { AxiosError, AxiosResponse } from "axios";
import { accessTokenHelper, getSecret } from "../utils/helpers";
import { Environments, Routes, StatusCodes } from "../enums";
import {
  ILoginValues,
  IPoint,
  IReverseResult,
  ISearchResult,
  IUser,
} from "../interfaces";
import { IRegisterValues } from "../interfaces/IRegisterValues";
import { toast } from "react-toastify";
import Router from "../../pages/Router";
import { IError } from "../interfaces/IError";
import Constants from "../utils/constants";

axios.defaults.baseURL = getSecret(Environments.BaseUrl) + Constants.ApiPrefix;

axios.defaults.withCredentials = true;

axios.interceptors.request.use((config) => {
  const accessToken = accessTokenHelper.getAccessToken();

  if (accessToken) {
    config.headers.Authorization = `Bearer ${accessToken}`;
  }

  return config;
});

axios.interceptors.response.use(undefined, (error: AxiosError) => {
  if (error.message === "Network Error" && !error.response) {
    toast.error("Network error - make sure API is running!");
  }

  if (!error.response) {
    return Promise.reject(error);
  }

  const { status, data } = error.response;

  switch (status) {
    case StatusCodes.BadRequest: {
      const errors = data as IError[];
      const parsedErrors = errors.map((error) => error.message).join("\n");
      toast.error(parsedErrors, { style: Constants.MultilineToast });
      break;
    }

    case StatusCodes.Unauthorized:
      return refreshToken(error);

    case StatusCodes.NotFound:
      toast.warning("Resource not found");
      break;

    case StatusCodes.InternalServerError:
      Router.navigate(Routes.ServerError);
      break;
  }

  return Promise.reject(error);
});

const refreshToken = async (error: AxiosError) => {
  const isTokenStored = accessTokenHelper.isAccessTokenPresent();

  if (!isTokenStored || !error.config || error.config.url === "/auth/refresh") {
    accessTokenHelper.removeAccessToken();
    toast.error("Your session has expired, please login again");
    Router.navigate(Routes.Login);
    return Promise.reject(error);
  }

  try {
    const response = await agent.Auth.refresh();
    accessTokenHelper.setAccessToken(response.accessToken);

    const config = { ...error.config };
    config.headers.Authorization = `Bearer ${response.accessToken}`;

    return axios.request(config);
  } catch (error) {
    accessTokenHelper.removeAccessToken();
    toast.error("Your session has expired, please login again");
    Router.navigate(Routes.Login);
    return Promise.reject(error);
  }
};

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
  get: <T>(url: string) => axios.get<T>(url).then(responseBody),
  post: <T>(url: string, body: object, signal?: AbortSignal) =>
    axios.post<T>(url, body, { signal: signal }).then(responseBody),
  put: <T>(url: string, body: object) =>
    axios.put<T>(url, body).then(responseBody),
  delete: <T>(url: string) => axios.delete<T>(url).then(responseBody),
};

const Auth = {
  login: (values: ILoginValues) => requests.post<IUser>("/auth/login", values),
  register: (values: IRegisterValues) =>
    requests.post<IUser>("/auth/register", values),
  refresh: () => {
    const accessToken = accessTokenHelper.getAccessToken();
    return requests.post<IUser>("/auth/refresh", { accessToken });
  },
  confirmEmail: (id: string, token: string, signal: AbortSignal) =>
    requests.post("/auth/confirm-email", { id, token }, signal),
};

const Map = {
  search: (query: string) =>
    requests.get<ISearchResult>(`/map/search?query=${query}`),
  reverse: (point: IPoint) =>
    requests.get<IReverseResult>(`/map/reverse?x=${point.x}&y=${point.y}`),
};

const agent = {
  Auth,
  Map,
};

export default agent;
