import { IPoint } from "../../../../common/interfaces";

export interface ISelectMarkerProps {
  onSelect: (point: IPoint) => void;
  isSelectBlocked: boolean;
}
