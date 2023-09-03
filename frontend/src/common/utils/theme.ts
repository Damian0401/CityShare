import { StyleFunctionProps, extendBaseTheme } from "@chakra-ui/react";
import chakraTheme from "@chakra-ui/theme";
import { ColorModes } from "../enums";
import {
  ChakraTheme,
  CommonTheme,
  LeafletTheme,
  NavbarContainer,
  PrimaryContainer,
  SecondaryContainer,
  TertiaryContainer,
} from "./styleConfigs";

const {
  Button,
  Input,
  FormError,
  Modal,
  Divider,
  Tooltip,
  Select,
  Checkbox,
  Textarea,
} = chakraTheme.components;

const theme = extendBaseTheme({
  config: {
    initialColorMode: ColorModes.Light,
  },
  styles: {
    global: (props: StyleFunctionProps) => ({
      ...CommonTheme(props),
      ...LeafletTheme(props),
      ...ChakraTheme(props),
    }),
  },
  components: {
    Input,
    Select,
    Textarea,
    Button,
    FormError,
    Modal,
    Divider,
    Tooltip,
    Checkbox,
    PrimaryContainer,
    SecondaryContainer,
    TertiaryContainer,
    NavbarContainer,
  },
});

export default theme;
