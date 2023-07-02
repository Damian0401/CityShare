import { extendBaseTheme } from "@chakra-ui/react";
import chakraTheme from "@chakra-ui/theme";
import { ColorModes } from "../enums";
import { PrimaryContainer, SecondaryContainer } from "./styleConfigs";

const { Button } = chakraTheme.components;

const theme = extendBaseTheme({
  config: {
    initialColorMode: ColorModes.Light,
  },
  components: {
    Button,
    PrimaryContainer,
    SecondaryContainer
  },
});

export default theme;
