import {
  IComment,
  IEvent,
  IEventCreateValues,
  IEventImage,
  IEventSearchQuery,
  IPageWrapper,
} from "../interfaces";
import { correctCommentDate, correctEventDates } from "../utils/helpers";
import requests from "../utils/requests";

const Event = {
  get: async (query: IEventSearchQuery, signal?: AbortSignal) => {
    let url = `/events?`;

    for (const [key, value] of Object.entries(query)) {
      if (!value) {
        continue;
      }

      if (Array.isArray(value) && value.length === 0) {
        continue;
      }

      if (Array.isArray(value) && value.length > 0) {
        url += `${key}=${value.join(",")}&`;
        continue;
      }

      if (value instanceof Date) {
        url += `${key}=${value.toISOString()}&`;
        continue;
      }

      url += `${key}=${value}&`;
    }

    const response = await requests.get<IPageWrapper<IEvent>>(url, signal);

    for (const event of response.content) {
      correctEventDates(event);
    }

    return response;
  },
  getById: async (id: string, signal?: AbortSignal) => {
    const event = await requests.get<IEvent>(`/events/${id}`, signal);

    correctEventDates(event);

    return event;
  },
  create: (values: IEventCreateValues) =>
    requests.post<string>("/events", values),
  uploadImage: (id: string, image: IEventImage) => {
    let url = `/events/${id}/images`;

    if (image.shouldBeBlurred) {
      url += "?shouldBeBlurred=true";
    }

    const formData = new FormData();
    formData.append("image", image.file);

    return requests.post(url, formData);
  },
  updateLikes: (id: string) => requests.post(`/events/${id}/likes`),
  addComment: (id: string, comment: string) =>
    requests.post(`/events/${id}/comments`, { message: comment }),
  getComments: async (id: string) => {
    const comments = await requests.get<IComment[]>(`/events/${id}/comments`);

    for (const comment of comments) {
      correctCommentDate(comment);
    }

    return comments;
  },
};

export default Event;
