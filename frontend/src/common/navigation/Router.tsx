import { createBrowserRouter } from "react-router-dom";
import PageWrapper from "../../components/page-wrapper/PageWrapper";
import Navbar from "../../components/navbar/Navbar";
import Home from "../../pages/home/Home";
import Routes from "./Routes";

const Router = createBrowserRouter([
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

export default Router;
