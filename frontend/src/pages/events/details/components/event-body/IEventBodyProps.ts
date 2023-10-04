import { IEvent } from "../../../../../common/interfaces";

export interface IEventBodyProps {
  event: IEvent;
  onLikeClick: (eventId: string) => void;
}
