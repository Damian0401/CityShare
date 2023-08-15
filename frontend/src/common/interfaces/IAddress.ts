import { IBoundingBox, IPoint } from ".";

export interface IAddress {
  id: number;
  displayName: string;
  point: IPoint;
  boundingBox: IBoundingBox;
}
