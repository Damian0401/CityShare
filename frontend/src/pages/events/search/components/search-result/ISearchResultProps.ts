import { IEvent } from "../../../../../common/interfaces";

export interface ISearchResultProps {
  events: IEvent[];
  onLikeClick: (eventId: string, isLiked: boolean) => void;
}
