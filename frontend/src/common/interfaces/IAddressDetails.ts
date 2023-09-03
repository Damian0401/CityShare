import { IBoundingBox, IPoint } from ".";

export interface IAddressDetails {
  displayName: string;
  point: IPoint;
  boundingBox: IBoundingBox;
}
