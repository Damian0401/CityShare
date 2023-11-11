import axios, { AxiosResponse } from "axios";

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
  get: <T>(url: string, signal?: AbortSignal) =>
    axios.get<T>(url, { signal: signal }).then(responseBody),
  post: <T>(url: string, body?: object, signal?: AbortSignal) =>
    axios.post<T>(url, body, { signal: signal }).then(responseBody),
  put: <T>(url: string, body: object) =>
    axios.put<T>(url, body).then(responseBody),
  delete: <T>(url: string) => axios.delete<T>(url).then(responseBody),
};

export default requests;
