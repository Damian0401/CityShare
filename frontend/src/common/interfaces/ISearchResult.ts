import { IBoundingBox } from ".";

export interface ISearchResult {
  displayName: string;
  place: string;
  x: number;
  y: number;
  boundingBox: IBoundingBox;
}
