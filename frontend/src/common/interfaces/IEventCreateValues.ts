import { IAddress, IEventImage } from ".";

export interface IEventCreateValues {
  title: string;
  description: string;
  cityId: number;
  address: IAddress;
  categoryIds: number[];
  images?: IEventImage[];
  startDate: Date;
  endDate: Date;
}
