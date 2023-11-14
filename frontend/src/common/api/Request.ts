import {
  IRequest,
  IRequestCreateValue,
  IRequestType,
  IRequests,
} from "../interfaces";
import { correctRequestDate } from "../utils/helpers";
import requests from "../utils/requests";

const Request = {
  getTypes: () => requests.get<IRequestType[]>("/requests/types"),
  getRequestsByCityId: async (cityId: number, signal?: AbortSignal) => {
    const data = await requests.get<IRequests>(
      `/requests?cityId=${cityId}`,
      signal
    );

    const newMap = new Map<number, IRequest[]>();

    for (const [key, value] of Object.entries(data.requests)) {
      correctRequestDate(value);
      newMap.set(+key, value);
    }

    data.requests = newMap;

    return data;
  },
  create: (values: IRequestCreateValue) => requests.post("/requests", values),
  accept: (id: string) => requests.post(`/requests/${id}/accept`),
  reject: (id: string) => requests.post(`/requests/${id}/reject`),
};

export default Request;
