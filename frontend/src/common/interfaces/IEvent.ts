import { IAddress } from ".";

export interface IEvent {
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
  author: string;
  comments: number;
  isLiked?: boolean;
}
