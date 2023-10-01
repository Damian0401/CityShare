import { makeAutoObservable, runInAction } from "mobx";
import { ICategory, ICity } from "../interfaces";
import agent from "../api/agent";

export default class CommonStore {
  cities: ICity[] = [];
  categories: ICategory[] = [];

  constructor() {
    makeAutoObservable(this);
  }

  loadCommonData = async () => {
    const citiesPromise = async () => {
      const cities = await agent.City.get();
      runInAction(() => {
        this.cities = cities;
      });
    };

    const categoriesPromise = async () => {
      const categories = await agent.Category.get();
      runInAction(() => {
        this.categories = categories;
      });
    };

    return Promise.all([citiesPromise(), categoriesPromise()]);
  };
}
