import { IAddress, IAddressDetails, IPoint } from "../interfaces";
import requests from "../utils/requests";

const Map = {
  search: (query: string) =>
    requests.get<IAddressDetails>(`/map/search?query=${query}`),
  reverse: (point: IPoint) =>
    requests.get<IAddress>(`/map/reverse?x=${point.x}&y=${point.y}`),
};

export default Map;
