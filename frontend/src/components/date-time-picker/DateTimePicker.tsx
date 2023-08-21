import {
  FormControl,
  FormErrorMessage,
  FormLabel,
  Input,
} from "@chakra-ui/react";
import { IDateTimePickerProps } from "./IDateTimePickerProps";
import { Field } from "formik";
import { InputTypes } from "../../common/enums";

const DateTimePicker: React.FC<IDateTimePickerProps> = (props) => {
  const { name, label, isRequired, errors, touched } = props;

  const handleDateClick = (
    e: React.MouseEvent<HTMLInputElement, MouseEvent>
  ) => {
    e.currentTarget.showPicker();
  };

  return (
    <FormControl isRequired={isRequired} isInvalid={!!touched && !!errors}>
      <FormLabel>{label}</FormLabel>
      <Field
        as={Input}
        type={InputTypes.DateTime}
        name={name}
        onClick={handleDateClick}
      />
      <FormErrorMessage>{errors}</FormErrorMessage>
    </FormControl>
  );
};

export default DateTimePicker;
