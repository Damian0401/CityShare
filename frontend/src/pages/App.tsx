import { RouterProvider, createBrowserRouter } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import PageWrapper from "../components/page-wrapper/PageWrapper";
import Navbar from "../components/navbar/Navbar";
import { ToastPositions } from "../common/enums";
import { ChakraBaseProvider } from "@chakra-ui/react";
import Routes from "../common/navigation/Routes";
import theme from "../common/utils/theme";

function App() {
  const router = createBrowserRouter([
    {
      path: Routes.Index,
      element: <PageWrapper Element={Navbar} />,
      children: [
        { index: true, element: <div>Home</div> },
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
