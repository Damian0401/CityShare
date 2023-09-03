import { IEvent } from "../../../../../common/interfaces";

export interface IMapMarkerProps {
  event: IEvent;
  onLikeClick: (eventId: number, isLiked: boolean) => void;
}
