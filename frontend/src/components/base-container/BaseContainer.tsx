import { Box, useStyleConfig } from "@chakra-ui/react";
import { IBaseContainerProps } from "./IBaseContainerProps";
import styles from "./BaseContainer.module.scss";

const BaseContainer: React.FC<IBaseContainerProps> = ({ type, children }) => {
  const container = useStyleConfig(type);
  return (
    <Box className={styles.container} __css={container}>
      {children}
    </Box>
  );
};

export default BaseContainer;
