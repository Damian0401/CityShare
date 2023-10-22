import { IBlurRequest } from "../../../../common/interfaces/IBlurRequest";

export interface ICollapsableRequestProps {
  request: IBlurRequest;
  onAccept: (requestId: string) => void;
  onReject: (requestId: string) => void;
}
