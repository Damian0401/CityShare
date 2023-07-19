import React from "react";
import ReactDOM from "react-dom/client";
import "react-toastify/dist/ReactToastify.min.css";
import { StoreContext, store } from "./common/stores/store.ts";
import { ChakraBaseProvider } from "@chakra-ui/react";
import theme from "./common/utils/theme.ts";
import { RouterProvider } from "react-router-dom";
import Router from "./pages/Router.tsx";
import { ToastContainer } from "react-toastify";
import { ToastPositions } from "./common/enums/index.ts";

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <StoreContext.Provider value={store}>
      <ChakraBaseProvider theme={theme}>
        <RouterProvider router={Router} />
        <ToastContainer position={ToastPositions.BottomRight} hideProgressBar />
      </ChakraBaseProvider>
    </StoreContext.Provider>
  </React.StrictMode>
);
