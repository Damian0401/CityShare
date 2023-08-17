import { ICreatePostValues, ILoginValues } from ".";
import { IRegisterValues } from "./IRegisterValues";

export interface IInitialValues {
  Login: ILoginValues;
  Register: IRegisterValues;
  PostCreate: ICreatePostValues;
}
