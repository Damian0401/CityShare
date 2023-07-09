import { Secrets } from "../enums";

export const getSecret = (secret: Secrets) => {
  return process.env[secret];
};
