import { IAddress, IImage } from ".";

export interface IEvent {
  id: string;
  title: string;
  description: string;
  address: IAddress;
  cityId: number;
  categoryIds: number[];
  images: IImage[];
  startDate: Date;
  endDate: Date;
  createdAt: Date;
  likes: number;
  author: string;
  comments: number;
  isLiked: boolean;
}
