import { IAddress } from "../../../../../common/interfaces";

export interface IAddressPickerModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSelect: (result: IAddress) => void;
  cityId: number | string;
}
