import { ChakraSizes } from "../../common/enums";
import { IPoint, IReverseResult } from "../../common/interfaces";

export interface IAddressSearchMapProps {
  initialPoint: IPoint;
  searchInputSize?: ChakraSizes;
  additionalQuery?: string;
  onSelect: (result: IReverseResult) => void;
}
