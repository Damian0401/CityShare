import { ICity } from "../interfaces";
import requests from "../utils/requests";

const City = {
  get: () => requests.get<ICity[]>("/cities"),
};

export default City;
