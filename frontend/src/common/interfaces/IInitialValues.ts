import {
  IPostCreateValues,
  ILoginValues,
  IRegisterValues,
  IPostSearchQuery,
} from ".";

export interface IInitialValues {
  Login: ILoginValues;
  Register: IRegisterValues;
  PostCreate: IPostCreateValues;
  PostSearch: IPostSearchQuery;
}
