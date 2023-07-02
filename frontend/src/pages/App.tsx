import { RouterProvider, createBrowserRouter } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import PageWrapper from "../components/page-wrapper/PageWrapper";
import Navbar from "../components/navbar/Navbar";
import { ToastPositions } from "../common/enums";
import { ChakraBaseProvider } from "@chakra-ui/react";
import Routes from "../common/navigation/Routes";
import theme from "../common/utils/theme";
import Home from "./home/Home";

function App() {
  const router = createBrowserRouter([
    {
      path: Routes.Index,
      element: <PageWrapper Element={Navbar} />,
      children: [
        { index: true, element: <Home /> },
        { path: Routes.Login, element: <div>Login</div> },
        { path: Routes.Register, element: <div>Register</div> },
      ],
    },
  ]);

  return (
    <>
      <ChakraBaseProvider theme={theme}>
        <RouterProvider router={router} />
        <ToastContainer position={ToastPositions.BottomRight} hideProgressBar />
      </ChakraBaseProvider>
    </>
  );
}

export default App;
