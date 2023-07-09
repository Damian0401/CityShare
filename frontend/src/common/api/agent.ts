import axios, { AxiosResponse } from "axios";
import { getSecret } from "../utils/helpers";
import { Secrets, StorageKeys } from "../enums";

axios.defaults.baseURL = getSecret(Secrets.BaseUrl) + "/api";

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

const agent = {};

export default agent;
