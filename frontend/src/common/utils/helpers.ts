import { format } from "date-fns";
import { Environments, StorageKeys } from "../enums";
import { IEvent } from "../interfaces";

export const getSecret = (environment: Environments) => {
  return import.meta.env[environment];
};

export const accessTokenHelper = {
  isAccessTokenPresent: () => {
    return !!localStorage.getItem(StorageKeys.AccessToken);
  },

  removeAccessToken: () => {
    localStorage.removeItem(StorageKeys.AccessToken);
  },

  setAccessToken: (accessToken: string) => {
    localStorage.setItem(StorageKeys.AccessToken, accessToken);
  },

  getAccessToken: () => {
    return localStorage.getItem(StorageKeys.AccessToken);
  },
};

export const correctEventDates = (event: IEvent) => {
  event.startDate = new Date(event.startDate);
  event.endDate = new Date(event.endDate);
  event.createdAt = new Date(event.createdAt);
};

export const importantStyle = (style: string) => {
  return `${style} !important`;
};

export const getFormattedDate = (date: Date) => {
  return format(date, "HH:mm dd.MM.yyyy");
};

export const updateLikes = (event: IEvent) => {
  if (event.isLiked) {
    event.likes -= 1;
    event.isLiked = false;
    return;
  }

  event.likes += 1;
  event.isLiked = true;
};
