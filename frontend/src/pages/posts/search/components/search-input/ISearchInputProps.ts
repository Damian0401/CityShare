import { IPostSearchQuery } from "../../../../../common/interfaces";

export interface ISearchInputProps {
  onSearch: (query: IPostSearchQuery) => void;
}
