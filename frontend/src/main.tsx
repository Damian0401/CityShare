import React from "react";
import ReactDOM from "react-dom/client";
import "react-toastify/dist/ReactToastify.min.css";
import App from "./pages/App.tsx";
import { StoreContext, store } from "./common/stores/store.ts";
import { ChakraBaseProvider } from "@chakra-ui/react";
import theme from "./common/utils/theme.ts";

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <StoreContext.Provider value={store}>
      <ChakraBaseProvider theme={theme}>
        <App />
      </ChakraBaseProvider>
    </StoreContext.Provider>
  </React.StrictMode>
);
