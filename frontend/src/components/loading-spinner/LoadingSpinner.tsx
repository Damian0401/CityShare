import { Spinner } from "@chakra-ui/react";

const LoadingSpinner = () => {
  return (
    <Spinner
      thickness="6px"
      speed="1s"
      emptyColor="gray.200"
      color="blue.500"
      height="20vh"
      width="20vh"
    />
  );
};

export default LoadingSpinner;
