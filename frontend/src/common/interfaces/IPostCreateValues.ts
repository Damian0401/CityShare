import { IAddress, INewImage } from ".";

export interface IPostCreateValues {
  title: string;
  description: string;
  cityId: number;
  address: IAddress;
  categoryIds: number[];
  images: INewImage[];
  startDate: Date;
  endDate: Date;
}
