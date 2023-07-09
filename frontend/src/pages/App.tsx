import { RouterProvider } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import Router from "../common/navigation/Router";
import { ToastPositions } from "../common/enums";

const App = () => {
  return (
    <>
      <RouterProvider router={Router} />
      <ToastContainer position={ToastPositions.BottomRight} hideProgressBar />
    </>
  );
};

export default App;
