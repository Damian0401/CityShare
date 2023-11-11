import axios, { AxiosError } from "axios";
import { AccessTokenHelper, getSecret } from "../utils/helpers";
import { Environments, Routes, StatusCodes } from "../enums";
import { toast } from "react-toastify";
import Router from "../../pages/Router";
import { IError } from "../interfaces/IError";
import Constants from "../utils/constants";
import Auth from "./Auth";
import Maps from "./Map";
import Category from "./Category";
import City from "./City";
import Event from "./Event";
import Request from "./Request";

axios.defaults.baseURL = getSecret(Environments.BaseUrl) + Constants.ApiPrefix;

axios.defaults.withCredentials = true;

axios.interceptors.request.use((config) => {
  const accessToken = AccessTokenHelper.getAccessToken();

  if (accessToken) {
    config.headers.Authorization = `Bearer ${accessToken}`;
  }

  return config;
});

axios.interceptors.response.use(undefined, (error: AxiosError) => {
  if (error.message === Constants.NetworkError && !error.response) {
    toast.error("Network error - make sure API is running!");
  }

  if (!error.response) {
    return Promise.reject(error);
  }

  const { status, data } = error.response;

  if (status === StatusCodes.BadRequest && !data) {
    Router.navigate(Routes.ServerError);
    return Promise.reject(error);
  }

  switch (status) {
    case StatusCodes.BadRequest: {
      const errors = data as IError[];
      const parsedErrors = errors.map((error) => error.message).join("\n");
      toast.error(parsedErrors, { style: Constants.MultilineToast });
      break;
    }

    case StatusCodes.Unauthorized:
      return refreshToken(error);

    case StatusCodes.NotFound:
      toast.warning("Resource not found");
      break;

    case StatusCodes.InternalServerError:
      Router.navigate(Routes.ServerError);
      break;
  }

  return Promise.reject(error);
});

const refreshToken = async (error: AxiosError) => {
  const isTokenStored = AccessTokenHelper.isAccessTokenPresent();

  if (!isTokenStored || !error.config || error.config.url === "/auth/refresh") {
    AccessTokenHelper.removeAccessToken();
    toast.error("Your session has expired, please login again");
    Router.navigate(Routes.Login);
    return Promise.reject(error);
  }

  try {
    const response = await agent.Auth.refresh();
    AccessTokenHelper.setAccessToken(response.accessToken);

    const config = { ...error.config };
    config.headers.Authorization = `Bearer ${response.accessToken}`;

    return axios.request(config);
  } catch (error) {
    AccessTokenHelper.removeAccessToken();
    toast.error("Your session has expired, please login again");
    Router.navigate(Routes.Login);
    return Promise.reject(error);
  }
};

const agent = {
  Auth,
  Map: Maps,
  Category,
  City,
  Event,
  Request,
};

export default agent;
