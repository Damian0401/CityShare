import { Environments } from "../enums";

export const getSecret = (environment: Environments) => {
  return import.meta.env[environment];
};
