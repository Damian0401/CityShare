import { IBoundingBox, IPoint } from ".";

export interface ISearchResult {
  displayName: string;
  point: IPoint;
  boundingBox: IBoundingBox;
}
