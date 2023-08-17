import { IAddress } from ".";

export interface ICreatePostValues {
  title: string;
  description: string;
  cityId: number;
  address: IAddress;
  categoryIds: number[];
  images: File[];
}
