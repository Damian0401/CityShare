import { MoonIcon, SunIcon } from "@chakra-ui/icons";
import { IconButton, useColorMode } from "@chakra-ui/react";
import { ButtonVariants, ColorModes } from "../../common/enums";
import { ThemeButtonLabel } from "../../common/utils/constants";

const ToggleThemeButton = () => {
  const { colorMode, toggleColorMode } = useColorMode();

  const isLightMode = colorMode === ColorModes.Light;

  const icon = isLightMode ? <MoonIcon boxSize={7} /> : <SunIcon boxSize={7} />;

  return (
    <IconButton
      variant={ButtonVariants.Ghost}
      aria-label={ThemeButtonLabel}
      _hover={{ opacity: "0.5" }}
      _active={{}}
      icon={icon}
      onClick={toggleColorMode}
    />
  );
};

export default ToggleThemeButton;
