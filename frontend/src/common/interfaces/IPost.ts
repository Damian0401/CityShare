import { IAddress } from ".";

export interface IPost {
  id: number;
  title: string;
  description: string;
  address: IAddress;
  cityId: number;
  categoryIds: number[];
  imageUrl: string;
  startDate: Date;
  endDate: Date;
}
