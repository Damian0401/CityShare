import { IAddress } from ".";

export interface IPost {
  id: number;
  title: string;
  description: string;
  address: IAddress;
  cityId: number;
  categoryIds: number[];
  imageUrls: string[];
  startDate: Date;
  endDate: Date;
  createdAt: Date;
  likes: number;
  isLiked?: boolean;
}
