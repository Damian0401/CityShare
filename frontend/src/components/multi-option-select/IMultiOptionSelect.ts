import { IOption } from "../../common/interfaces";

export interface IMultiOptionSelectProps {
  errors?: string | string[];
  touched?: boolean;
  name: string;
  label: string;
  options: IOption[];
  isRequired?: boolean;
  onChange: (value: (number | string)[]) => void;
}
