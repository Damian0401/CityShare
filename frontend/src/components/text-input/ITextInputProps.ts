export interface ITextInputProps {
  errors?: string;
  touched?: boolean;
  name: string;
  type: string;
  label: string;
  isRequired?: boolean;
  placeholder?: string;
  isDisabled?: boolean;
  isMultiline?: boolean;
  isReadOnly?: boolean;
  onClick?: () => void;
}
