import { Box, useStyleConfig } from "@chakra-ui/react";
import { IBaseContainerProps } from "./IBaseContainerProps";
import styles from "./BaseContainer.module.scss";

const BaseContainer: React.FC<IBaseContainerProps> = (props) => {
  const { children, className, type, onClick } = props;
  const container = useStyleConfig(type);
  return (
    <Box
      className={styles.container + " " + className}
      onClick={onClick}
      __css={container}
    >
      {children}
    </Box>
  );
};

export default BaseContainer;
