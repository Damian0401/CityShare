import { IPostCreateValues, ILoginValues } from ".";
import { IRegisterValues } from "./IRegisterValues";

export interface IInitialValues {
  Login: ILoginValues;
  Register: IRegisterValues;
  PostCreate: IPostCreateValues;
}
