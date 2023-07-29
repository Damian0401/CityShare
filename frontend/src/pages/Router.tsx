import { createBrowserRouter } from "react-router-dom";
import PageWrapper from "../components/page-wrapper/PageWrapper";
import Navbar from "../components/navbar/Navbar";
import Home from "./home/Home";
import Login from "./login/Login";
import Register from "./register/Register";
import { Routes } from "../common/enums";
import NotFound from "./not-found/NotFound";
import ServerError from "./server-error/ServerError";
import AnonymousRoute from "../components/anonymous-route/AnonymousRoute";
import PrivateRoute from "../components/private-route/PrivateRoute";

const Router = createBrowserRouter([
  {
    path: Routes.Index,
    element: <PageWrapper Element={Navbar} />,
    children: [
      {
        index: true,
        element: <PrivateRoute Component={Home} />,
      },
      {
        path: Routes.Login,
        element: <AnonymousRoute Component={Login} />,
      },
      {
        path: Routes.Register,
        element: <AnonymousRoute Component={Register} />,
      },
      {
        path: Routes.NotFound,
        element: <NotFound />,
      },
      {
        path: Routes.ServerError,
        element: <ServerError />,
      },
      {
        path: Routes.Wildcard,
        element: <NotFound />,
      },
    ],
  },
]);

export default Router;
