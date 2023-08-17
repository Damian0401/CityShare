import { StyleFunctionProps, extendBaseTheme } from "@chakra-ui/react";
import chakraTheme from "@chakra-ui/theme";
import { ColorModes } from "../enums";
import { mode } from "@chakra-ui/theme-tools";
import colors from "../../assets/styles/colors.module.scss";
import {
  LeafletTheme,
  NavbarContainer,
  PrimaryContainer,
  SecondaryContainer,
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
      body: {
        color: mode(colors.textLight, colors.textDark)(props),
        backgroundColor: mode(
          colors.backgroundLight,
          colors.backgroundDark
        )(props),
      },
      ...LeafletTheme(props),
    }),
  },
  components: {
    Button,
    Input: {
      baseStyle: (props: StyleFunctionProps) => ({
        field: {
          backgroundColor: mode(colors.inputLight, colors.inputDark)(props),
        },
      }),
      ...Input,
    },
    FormError,
    Modal,
    Divider,
    Tooltip,
    Select,
    Checkbox,
    Textarea,
    PrimaryContainer,
    SecondaryContainer,
    NavbarContainer,
  },
});

export default theme;
