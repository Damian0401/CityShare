import { Environments, StorageKeys } from "../enums";

export const getSecret = (environment: Environments) => {
  return import.meta.env[environment];
};

export const isAccessTokenPresent = () => {
  return !!localStorage.getItem(StorageKeys.AccessToken);
};

export const removeAccessToken = () => {
  localStorage.removeItem(StorageKeys.AccessToken);
};

export const setAccessToken = (accessToken: string) => {
  localStorage.setItem(StorageKeys.AccessToken, accessToken);
};

export const getAccessToken = () => {
  return localStorage.getItem(StorageKeys.AccessToken);
};
