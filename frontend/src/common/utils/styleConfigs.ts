import { StyleFunctionProps, defineStyleConfig } from "@chakra-ui/react";
import { mode } from "@chakra-ui/theme-tools";
import colors from "../../assets/styles/colors.module.scss";

export const PrimaryContainer = defineStyleConfig({
  baseStyle: (props: StyleFunctionProps) => ({
    backgroundColor: mode(colors.primaryLight, colors.primaryDark)(props),
  }),
});

export const SecondaryContainer = defineStyleConfig({
  baseStyle: (props: StyleFunctionProps) => ({
    backgroundColor: mode(colors.secondaryLight, colors.secondaryDark)(props),
  }),
});

export const NavbarContainer = defineStyleConfig({
  baseStyle: (props: StyleFunctionProps) => ({
    backgroundColor: mode(colors.navbarLight, colors.navbarDark)(props),
  }),
});
