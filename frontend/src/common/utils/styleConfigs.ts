import { StyleFunctionProps, defineStyleConfig } from "@chakra-ui/react";
import { mode } from "@chakra-ui/theme-tools";
import colors from "../../assets/styles/colors.module.scss";
import { importantStyle } from "./helpers";

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

export const LeafletTheme = (props: StyleFunctionProps) => ({
  ".leaflet-container": {
    height: "100%",
    width: "100%",
  },
  ".leaflet-tile": {
    filter: mode(
      "none",
      "brightness(0.6) invert(1) contrast(3) hue-rotate(200deg) saturate(0.3) brightness(0.7)"
    )(props),
  },
  ".leaflet-control-zoom-in": {
    backgroundColor: mode(
      importantStyle(colors.backgroundLight),
      importantStyle(colors.backgroundDark)
    )(props),
    color: mode(
      importantStyle(colors.textLight),
      importantStyle(colors.textDark)
    )(props),
  },
  ".leaflet-control-zoom-out": {
    backgroundColor: mode(
      importantStyle(colors.backgroundLight),
      importantStyle(colors.backgroundDark)
    )(props),
    color: mode(
      importantStyle(colors.textLight),
      importantStyle(colors.textDark)
    )(props),
  },
});
