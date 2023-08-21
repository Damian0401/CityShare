import { ChakraSizes } from "../../common/enums";
import { IAddress, IPoint } from "../../common/interfaces";

export interface IAddressSearchMapProps {
  initialPoint: IPoint;
  searchInputSize?: ChakraSizes;
  additionalQuery?: string;
  isSearchOnly?: boolean;
  elements?: JSX.Element[];
  scrollToPoint?: IPoint;
  onSelect?: (result: IAddress) => void;
}
