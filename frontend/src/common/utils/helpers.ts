import { Environments, StorageKeys } from "../enums";

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
