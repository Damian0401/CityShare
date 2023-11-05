import { IRequest } from "../../../../common/interfaces";

export interface ICollapsableRequestProps {
  request: IRequest;
  onAccept: (requestId: string) => void;
  onReject: (requestId: string) => void;
}
