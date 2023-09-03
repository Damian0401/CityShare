import { IEvent } from "../../../../../common/interfaces";

export interface ISearchResultProps {
  events: IEvent[];
  onLikeClick: (eventId: number, isLiked: boolean) => void;
}
