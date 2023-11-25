import { SunIcon } from "@chakra-ui/icons";
import { Button, IconButton, useColorMode } from "@chakra-ui/react";
import { ColorModes } from "../../../../../../common/enums";
import { IToggleThemeButtonProps } from "./IToggleThemeButtonProps";
import { BsMoon } from "react-icons/bs";
import Constants from "../../../../../../common/utils/constants";

const ToggleThemeButton: React.FC<IToggleThemeButtonProps> = ({
  buttonRef,
  displayText = true,
}) => {
  const { colorMode, toggleColorMode } = useColorMode();

  const isLightMode = colorMode === ColorModes.Light;

  const icon = isLightMode ? <BsMoon /> : <SunIcon />;

  const text = isLightMode ? "Dark mode" : "Light mode";

  if (displayText) {
    return (
      <Button leftIcon={icon} onClick={toggleColorMode} ref={buttonRef}>
        {text}
      </Button>
    );
  }

  return (
    <IconButton
      aria-label={Constants.AriaLabels.ToggleThemeButton}
      icon={icon}
      onClick={toggleColorMode}
      ref={buttonRef}
      variant="ghost"
      fontSize="30px"
    />
  );
};

export default ToggleThemeButton;
