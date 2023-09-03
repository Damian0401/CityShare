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

export const importantStyle = (style: string) => {
  return `${style} !important`;
};

export const getFormattedDate = (date: Date) => {
  return format(date, "HH:mm dd.MM.yyyy");
};

export const updateLikes = (event: IEvent, isLiked: boolean) => {
  if (event.isLiked === isLiked) {
    event.isLiked = undefined;
    event.likes += isLiked ? -1 : 1;
  } else if (event.isLiked === undefined) {
    event.isLiked = isLiked;
    event.likes += isLiked ? 1 : -1;
  } else {
    event.isLiked = isLiked;
    event.likes += isLiked ? 2 : -2;
  }
};
