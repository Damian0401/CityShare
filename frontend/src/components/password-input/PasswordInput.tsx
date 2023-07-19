import {
  FormControl,
  FormErrorMessage,
  FormLabel,
  Input,
  InputGroup,
  InputRightElement,
} from "@chakra-ui/react";
import * as React from "react";
import { IPasswordInputProps } from "./IPasswordInputProps";
import { ViewIcon, ViewOffIcon } from "@chakra-ui/icons";
import { Cursors, InputTypes } from "../../common/enums";
import { Field } from "formik";

const PasswordInput: React.FC<IPasswordInputProps> = (props) => {
  const { name, label, errors, touched, isRequired } = props;

  const [show, setShow] = React.useState(false);
  const icon = show ? <ViewIcon /> : <ViewOffIcon />;
  const handleClick = () => setShow(!show);

  return (
    <FormControl isInvalid={!!errors && !!touched} isRequired={isRequired}>
      <FormLabel htmlFor={name}>{label}</FormLabel>
      <InputGroup>
        <Field
          as={Input}
          id={name}
          name={name}
          type={show ? InputTypes.Text : InputTypes.Password}
        />
        <InputRightElement
          children={icon}
          onClick={handleClick}
          cursor={Cursors.Pointer}
        />
      </InputGroup>
      <FormErrorMessage>{errors}</FormErrorMessage>
    </FormControl>
  );
};

export default PasswordInput;
