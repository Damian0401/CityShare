import {
  FormControl,
  FormErrorMessage,
  FormLabel,
  Input,
} from "@chakra-ui/react";
import { IDateTimePickerProps } from "./IDateTimePickerProps";
import { InputTypes } from "../../common/enums";
import styles from "./DateTimePicker.module.scss";

const DateTimePicker: React.FC<IDateTimePickerProps> = (props) => {
  const { label, isRequired, errors, touched, onChange } = props;

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
        onChange={(e) => onChange(new Date(e.target.value))}
      />
      <FormErrorMessage>{errors}</FormErrorMessage>
    </FormControl>
  );
};

export default DateTimePicker;
