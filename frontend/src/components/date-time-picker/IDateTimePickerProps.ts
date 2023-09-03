export interface IDateTimePickerProps {
  errors?: string;
  touched?: boolean;
  label: string;
  isRequired?: boolean;
  defaultValue?: Date;
  onChange: (date: Date) => void;
}
