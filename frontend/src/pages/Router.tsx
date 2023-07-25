import { createBrowserRouter } from "react-router-dom";
import PageWrapper from "../components/page-wrapper/PageWrapper";
import Navbar from "../components/navbar/Navbar";
import Home from "./home/Home";
import Login from "./login/Login";
import Register from "./register/Register";
import { Routes } from "../common/enums";
import NotFound from "./not-found/NotFound";
import ServerError from "./server-error/ServerError";

const Router = createBrowserRouter([
  {
    path: Routes.Index,
    element: <PageWrapper Element={Navbar} />,
    children: [
      { index: true, element: <Home /> },
      { path: Routes.Login, element: <Login /> },
      { path: Routes.Register, element: <Register /> },
      { path: Routes.NotFound, element: <NotFound /> },
      { path: Routes.ServerError, element: <ServerError /> },
      { path: Routes.Wildcard, element: <NotFound /> },
    ],
  },
]);

export default Router;
