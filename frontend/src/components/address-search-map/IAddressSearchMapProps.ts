import { ChakraSizes } from "../../common/enums/ChakraSizes";
import { IPoint, IReverseResult } from "../../common/interfaces";

export interface IAddressSearchMapProps {
  initialPoint: IPoint;
  searchInputSize?: ChakraSizes;
  onSelect: (result: IReverseResult) => void;
}
