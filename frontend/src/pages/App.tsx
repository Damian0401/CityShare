import { RouterProvider, createBrowserRouter } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import PageWrapper from "../components/page-wrapper/PageWrapper";
import Navbar from "../components/navbar/Navbar";
import { Routes, ToastPositions } from "../common/enums";

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
      <RouterProvider router={router} />
      <ToastContainer position={ToastPositions.BottomRight} hideProgressBar />
    </>
  );
}

export default App;
