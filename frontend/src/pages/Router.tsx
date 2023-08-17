import { Navigate, createBrowserRouter } from "react-router-dom";
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
import ConfirmEmail from "./confirm-email/ConfirmEmail";
import PostMap from "./posts/map/PostMap";
import PostDetails from "./posts/details/PostDetails";
import PostCreate from "./posts/create/PostCreate";
import PostSearch from "./posts/search/PostSearch";

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
        path: Routes.Posts,
        children: [
          {
            path: Routes.PostsSearch,
            element: <PrivateRoute Component={PostSearch} />,
          },
          {
            path: Routes.PostsCreate,
            element: <PrivateRoute Component={PostCreate} />,
          },
          {
            path: Routes.PostsMap,
            element: <PrivateRoute Component={PostMap} />,
          },
          {
            path: Routes.PostsDetails,
            element: <PrivateRoute Component={PostDetails} />,
          },
        ],
      },
      {
        path: Routes.ConfirmEmail,
        element: <ConfirmEmail />,
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
        element: <Navigate to={Routes.NotFound} />,
      },
    ],
  },
]);

export default Router;
