import { makeAutoObservable } from "mobx";
import { ICategory, ICity } from "../interfaces";

export default class CommonStore {
  cities: ICity[] = [];
  categories: ICategory[] = [];

  constructor() {
    makeAutoObservable(this);
  }

  loadCommonData = async () => {
    this.cities = [
      {
        id: 1,
        name: "Wrocław",
        address: {
          displayName: "Wrocław, Polska",
          point: { x: 51.1089776, y: 17.0326689 },
          boundingBox: {
            maxX: 51.2100604,
            maxY: 17.1762192,
            minX: 51.0426686,
            minY: 16.8073393,
          },
        },
      },
      {
        id: 2,
        name: "Warszawa",
        address: {
          displayName: "Warszawa, Polska",
          point: { x: 52.2319581, y: 21.0067249 },
          boundingBox: {
            maxX: 52.3681531,
            maxY: 21.2711512,
            minX: 52.0978497,
            minY: 20.8516882,
          },
        },
      },
    ];

    this.categories = [
      {
        id: 1,
        name: "Sport",
        description: "Sport event",
      },
      {
        id: 2,
        name: "Music",
        description: "Music event",
      },
      {
        id: 3,
        name: "Theatre",
        description: "Theatre event",
      },
      {
        id: 4,
        name: "Cinema",
        description: "Cinema event",
      },
    ];

    return Promise.resolve();
  };
}
