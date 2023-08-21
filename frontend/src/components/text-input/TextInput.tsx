import {
  FormControl,
  FormErrorMessage,
  FormLabel,
  Input,
  InputGroup,
  InputRightElement,
  Textarea,
} from "@chakra-ui/react";
import { Field } from "formik";
import { ITextInputProps } from "./ITextInputProps";
import { useState } from "react";
import { ViewIcon, ViewOffIcon } from "@chakra-ui/icons";
import { Cursors, InputTypes } from "../../common/enums";

const TextInput: React.FC<ITextInputProps> = (props) => {
  const {
    name,
    label,
    errors,
    touched,
    isRequired,
    type,
    placeholder,
    isDisabled,
    isReadOnly,
    isMultiline,
    onClick,
  } = props;

  const [show, setShow] = useState(false);
  const icon = show ? <ViewIcon /> : <ViewOffIcon />;
  const handleClick = () => setShow(!show);

  return (
    <FormControl isInvalid={!!errors && !!touched} isRequired={isRequired}>
      <FormLabel htmlFor={name}>{label}</FormLabel>
      <InputGroup>
        <Field
          as={isMultiline ? Textarea : Input}
          id={name}
          name={name}
          type={show ? InputTypes.Text : type}
          placeholder={placeholder}
          isDisabled={isDisabled}
          isReadOnly={isReadOnly}
          onClick={onClick}
        />
        {type == InputTypes.Password && (
          <InputRightElement
            children={icon}
            onClick={handleClick}
            cursor={Cursors.Pointer}
          />
        )}
      </InputGroup>
      <FormErrorMessage>{errors}</FormErrorMessage>
    </FormControl>
  );
};

export default TextInput;
