import {
  IEventCreateValues,
  ILoginValues,
  IRegisterValues,
  IEventSearchQuery,
} from ".";

export interface IInitialValues {
  Login: ILoginValues;
  Register: IRegisterValues;
  EventCreate: IEventCreateValues;
  EventSearch: IEventSearchQuery;
}
