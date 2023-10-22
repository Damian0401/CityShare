import { SunIcon } from "@chakra-ui/icons";
import { Button, useColorMode } from "@chakra-ui/react";
import { ColorModes } from "../../../../../../common/enums";
import { IToggleThemeButtonProps } from "./IToggleThemeButtonProps";
import { BsMoon } from "react-icons/bs";

const ToggleThemeButton: React.FC<IToggleThemeButtonProps> = ({
  buttonRef: test,
}) => {
  const { colorMode, toggleColorMode } = useColorMode();

  const isLightMode = colorMode === ColorModes.Light;

  const icon = isLightMode ? <BsMoon /> : <SunIcon />;

  const text = isLightMode ? "Dark mode" : "Light mode";

  return (
    <Button leftIcon={icon} onClick={toggleColorMode} ref={test}>
      {text}
    </Button>
  );
};

export default ToggleThemeButton;
