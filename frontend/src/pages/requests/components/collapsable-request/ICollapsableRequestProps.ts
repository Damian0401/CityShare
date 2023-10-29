import { IRequest } from "../../../../common/interfaces/IRequest";

export interface ICollapsableRequestProps {
  request: IRequest;
  onAccept: (requestId: string) => void;
  onReject: (requestId: string) => void;
}
