import { ChakraSizes } from "../../common/enums";
import { IAddress, IPoint } from "../../common/interfaces";

export interface IAddressSearchMapProps {
  initialPoint: IPoint;
  searchInputSize?: ChakraSizes;
  additionalQuery?: string;
  disableSelect?: boolean;
  elements?: JSX.Element[];
  scrollToPoint?: IPoint;
  isSearchVisible?: boolean;
  initialZoom?: number;
  onSelect?: (result: IAddress) => void;
}
