import { IEvent } from "../../../../../common/interfaces";

export interface IMapMarkerProps {
  event: IEvent;
  onLikeClick: (eventId: string) => void;
}
