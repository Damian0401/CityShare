import { IOption } from "../../common/interfaces";

export interface IOptionSelectProps {
  label?: string;
  name: string;
  options: IOption[];
  errors?: string;
  touched?: boolean;
  isRequired?: boolean;
  isDisabled?: boolean;
}
