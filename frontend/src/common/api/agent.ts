import axios, { AxiosResponse } from "axios";
import { getSecret } from "../utils/helpers";
import { Environments, StorageKeys } from "../enums";
import { ILoginValues } from "../interfaces";
import { IRegisterValues } from "../interfaces/IRegisterValues";
import { IUser } from "../interfaces/IUser";

axios.defaults.baseURL = getSecret(Environments.BaseUrl) + "/api/v1";

axios.defaults.withCredentials = true;

axios.interceptors.request.use((config) => {
  const accessToken = localStorage.getItem(StorageKeys.AccessToken);

  if (accessToken) {
    config.headers.Authorization = `Bearer ${accessToken}`;
  }

  return config;
});

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
  refresh: (accessToken: string) =>
    requests.post<IUser>("/auth/refresh", { accessToken }),
};

const agent = {
  Auth,
};

export default agent;
