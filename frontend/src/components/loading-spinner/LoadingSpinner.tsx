import { Spinner, useColorMode } from "@chakra-ui/react";
import { ColorModes } from "../../common/enums";

const LoadingSpinner = () => {
  const { colorMode } = useColorMode();

  const isLightMode = colorMode === ColorModes.Light;

  const color = isLightMode ? "blue.400" : "blue.700";

  const emptyColor = isLightMode ? "gray.100" : "gray.400";

  return (
    <Spinner
      thickness="6px"
      speed="1s"
      emptyColor={emptyColor}
      color={color}
      height="20vh"
      width="20vh"
    />
  );
};

export default LoadingSpinner;
