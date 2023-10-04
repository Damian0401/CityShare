import {
  FormControl,
  FormErrorMessage,
  FormLabel,
  Select,
} from "@chakra-ui/react";
import { IOptionSelectProps } from "./IOptionSelectProps";
import { Cursors } from "../../common/enums";
import { Field } from "formik";

const OptionSelect: React.FC<IOptionSelectProps> = (props) => {
  const { errors, touched, options, label, name, isRequired, isDisabled } =
    props;
  return (
    <FormControl
      isInvalid={!!errors && !!touched}
      isRequired={isRequired}
      isDisabled={isDisabled}
    >
      {label && <FormLabel htmlFor={name}>{label}</FormLabel>}
      <Field
        as={Select}
        name={name}
        style={{ cursor: isDisabled ? Cursors.NotAllowed : Cursors.Pointer }}
      >
        {options.map((option) => (
          <option key={option.value} value={option.value}>
            {option.label}
          </option>
        ))}
      </Field>
      <FormErrorMessage>{errors}</FormErrorMessage>
    </FormControl>
  );
};

export default OptionSelect;
