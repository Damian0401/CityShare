import {
  FormControl,
  FormErrorMessage,
  FormLabel,
  Input,
} from "@chakra-ui/react";
import { Field } from "formik";
import { ITextInputProps } from "./ITextInputProps";

const TextInput: React.FC<ITextInputProps> = (props) => {
  const { name, label, type, errors, touched, isRequired } = props;
  return (
    <FormControl isInvalid={!!errors && !!touched} isRequired={isRequired}>
      <FormLabel htmlFor={name}>{label}</FormLabel>
      <Field as={Input} id={name} name={name} type={type} />
      <FormErrorMessage>{errors}</FormErrorMessage>
    </FormControl>
  );
};

export default TextInput;
