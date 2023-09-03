import { MoonIcon, SunIcon } from "@chakra-ui/icons";
import { IconButton, useColorMode } from "@chakra-ui/react";
import { ChakraVariants, ColorModes } from "../../common/enums";
import Constants from "../../common/utils/constants";
import variables from "../../../src/assets/styles/variables.module.scss";

const ToggleThemeButton = () => {
  const { colorMode, toggleColorMode } = useColorMode();

  const isLightMode = colorMode === ColorModes.Light;

  const icon = isLightMode ? <MoonIcon boxSize={7} /> : <SunIcon boxSize={7} />;

  return (
    <IconButton
      variant={ChakraVariants.Ghost}
      aria-label={Constants.AriaLabels.ToggleThemeButton}
      _hover={{ opacity: variables.hoverOpacity }}
      _active={{}}
      icon={icon}
      onClick={toggleColorMode}
    />
  );
};

export default ToggleThemeButton;
