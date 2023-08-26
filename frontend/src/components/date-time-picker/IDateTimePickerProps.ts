export interface IDateTimePickerProps {
  errors?: string;
  touched?: boolean;
  label: string;
  isRequired?: boolean;
  onChange: (date: Date) => void;
}
