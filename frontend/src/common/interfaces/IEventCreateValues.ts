import { IAddress, INewImage } from ".";

export interface IEventCreateValues {
  title: string;
  description: string;
  cityId: number;
  address: IAddress;
  categoryIds: number[];
  images: INewImage[];
  startDate: Date;
  endDate: Date;
}
