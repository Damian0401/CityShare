import { ICategory } from "../interfaces";
import requests from "../utils/requests";

const Category = {
  get: () => requests.get<ICategory[]>("/categories"),
};

export default Category;
