import { IAddress } from ".";

export interface IEvent {
  id: string;
  title: string;
  description: string;
  address: IAddress;
  cityId: number;
  categoryIds: number[];
  imageUrls: (string | null)[];
  startDate: Date;
  endDate: Date;
  createdAt: Date;
  likes: number;
  author: string;
  comments: number;
  isLiked: boolean;
}
