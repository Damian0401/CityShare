import { StyleFunctionProps, extendBaseTheme } from "@chakra-ui/react";
import chakraTheme from "@chakra-ui/theme";
import { ColorModes } from "../enums";
import { mode } from "@chakra-ui/theme-tools";
import colors from "../../assets/styles/colors.module.scss";
import {
  NavbarContainer,
  PrimaryContainer,
  SecondaryContainer,
} from "./styleConfigs";

const { Button, Input, FormError } = chakraTheme.components;

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
    }),
  },
  components: {
    Button,
    Input,
    FormError,
    PrimaryContainer,
    SecondaryContainer,
    NavbarContainer,
  },
});

export default theme;
