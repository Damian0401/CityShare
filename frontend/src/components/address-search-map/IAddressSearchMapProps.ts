import { ChakraSizes } from "../../common/enums/ChakraSizes";
import { IPoint } from "../../common/interfaces";

export interface IAddressSearchMapProps {
  initialPoint: IPoint;
  searchInputSize?: ChakraSizes;
}
