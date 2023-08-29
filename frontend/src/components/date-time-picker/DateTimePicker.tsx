import {
  FormControl,
  FormErrorMessage,
  FormLabel,
  Input,
} from "@chakra-ui/react";
import { IDateTimePickerProps } from "./IDateTimePickerProps";
import { InputTypes } from "../../common/enums";
import styles from "./DateTimePicker.module.scss";
import { format } from "date-fns";

const DateTimePicker: React.FC<IDateTimePickerProps> = (props) => {
  const { label, isRequired, errors, touched, defaultValue, onChange } = props;

  const handleDateClick = (
    e: React.MouseEvent<HTMLInputElement, MouseEvent>
  ) => {
    e.currentTarget.showPicker();
  };

  return (
    <FormControl
      isRequired={isRequired}
      isInvalid={!!touched && !!errors}
      className={styles.container}
    >
      <FormLabel>{label}</FormLabel>
      <Input
        type={InputTypes.DateTime}
        onClick={handleDateClick}
        defaultValue={
          defaultValue ? format(defaultValue, "yyyy-MM-dd'T'HH:mm") : ""
        }
        onChange={(e) => onChange(new Date(e.target.value))}
      />
      <FormErrorMessage>{errors}</FormErrorMessage>
    </FormControl>
  );
};

export default DateTimePicker;
