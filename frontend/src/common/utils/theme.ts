import { extendBaseTheme } from "@chakra-ui/react";
import chakraTheme from "@chakra-ui/theme";
import { ColorModes } from "../enums";

const { Button } = chakraTheme.components;

const theme = extendBaseTheme({
  config: {
    initialColorMode: ColorModes.Light,
  },
  components: {
    Button,
  },
});

export default theme;
