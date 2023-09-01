import { IEventSearchQuery } from "../../../../../common/interfaces";

export interface ISearchInputProps {
  onSearch: (query: IEventSearchQuery) => void;
}
