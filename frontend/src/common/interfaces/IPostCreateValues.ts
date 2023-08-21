import { IAddress } from ".";

export interface IPostCreateValues {
  title: string;
  description: string;
  cityId: number;
  address: IAddress;
  categoryIds: number[];
  images: File[];
  startDate: Date;
  endDate: Date;
}
