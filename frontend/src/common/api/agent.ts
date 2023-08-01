import axios, { AxiosError, AxiosResponse } from "axios";
import {
  getAccessToken,
  getSecret,
  isAccessTokenPresent,
  removeAccessToken,
  setAccessToken,
} from "../utils/helpers";
import { Environments, Routes, StatusCodes, StorageKeys } from "../enums";
import { ILoginValues } from "../interfaces";
import { IRegisterValues } from "../interfaces/IRegisterValues";
import { IUser } from "../interfaces/IUser";
import { toast } from "react-toastify";
import Router from "../../pages/Router";
import { IError } from "../interfaces/IError";
import Constants from "../utils/constants";

axios.defaults.baseURL = getSecret(Environments.BaseUrl) + "/api/v1";

axios.defaults.withCredentials = true;

axios.interceptors.request.use((config) => {
  const accessToken = getAccessToken();

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
  const isTokenStored = isAccessTokenPresent();

  if (!isTokenStored || !error.config || error.config.url === "/auth/refresh") {
    removeAccessToken();
    toast.error("Your session has expired, please login again");
    Router.navigate(Routes.Login);
    return Promise.reject(error);
  }

  try {
    const response = await agent.Auth.refresh();
    setAccessToken(response.accessToken);

    const config = { ...error.config };
    config.headers.Authorization = `Bearer ${response.accessToken}`;

    return axios.request(config);
  } catch (error) {
    removeAccessToken();
    toast.error("Your session has expired, please login again");
    Router.navigate(Routes.Login);
    return Promise.reject(error);
  }
};

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
  get: <T>(url: string) => axios.get<T>(url).then(responseBody),
  post: <T>(url: string, body: object) =>
    axios.post<T>(url, body).then(responseBody),
  put: <T>(url: string, body: object) =>
    axios.put<T>(url, body).then(responseBody),
  delete: <T>(url: string) => axios.delete<T>(url).then(responseBody),
};

const Auth = {
  login: (values: ILoginValues) => requests.post<IUser>("/auth/login", values),
  register: (values: IRegisterValues) =>
    requests.post<IUser>("/auth/register", values),
  refresh: () => {
    const accessToken = localStorage.getItem(StorageKeys.AccessToken);
    return requests.post<IUser>("/auth/refresh", { accessToken });
  },
};

const agent = {
  Auth,
};

export default agent;
