import { makeAutoObservable, runInAction } from "mobx";
import { ICategory, ICity, IRequestType } from "../interfaces";
import agent from "../api/agent";

export default class CommonStore {
  cities: ICity[] = [];
  categories: ICategory[] = [];
  requestTypes: IRequestType[] = [];
  isContentLoading = false;

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

    const requestTypesPromise = async () => {
      const requestTypes = await agent.Requests.getTypes();
      runInAction(() => {
        this.requestTypes = requestTypes;
      });
    };

    return Promise.all([
      citiesPromise(),
      categoriesPromise(),
      requestTypesPromise(),
    ]);
  };

  setLoading = (state: boolean) => {
    runInAction(() => {
      this.isContentLoading = state;
    });
  };
}
