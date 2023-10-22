import { Navigate, createBrowserRouter } from "react-router-dom";
import PageWrapper from "../components/page-wrapper/PageWrapper";
import Navbar from "../components/navbar/Navbar";
import Home from "./home/Home";
import Login from "./login/Login";
import Register from "./register/Register";
import { Roles, Routes } from "../common/enums";
import NotFound from "./not-found/NotFound";
import ServerError from "./server-error/ServerError";
import AnonymousRoute from "../components/anonymous-route/AnonymousRoute";
import PrivateRoute from "../components/private-route/PrivateRoute";
import ConfirmEmail from "./confirm-email/ConfirmEmail";
import EventMap from "./events/map/EventMap";
import EventDetails from "./events/details/EventDetails";
import EventCreate from "./events/create/EventCreate";
import EventSearch from "./events/search/EventSearch";
import AdminPanel from "./admin-panel/AdminPanel";

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
        path: Routes.Events,
        children: [
          {
            path: Routes.EventsSearch,
            element: <PrivateRoute Component={EventSearch} />,
          },
          {
            path: Routes.EventsCreate,
            element: <PrivateRoute Component={EventCreate} />,
          },
          {
            path: Routes.EventsMap,
            element: <PrivateRoute Component={EventMap} />,
          },
          {
            path: Routes.EventsDetails,
            element: <PrivateRoute Component={EventDetails} />,
          },
        ],
      },
      {
        path: Routes.ConfirmEmail,
        element: <ConfirmEmail />,
      },
      {
        path: Routes.AdminPanel,
        element: <PrivateRoute Component={AdminPanel} Roles={[Roles.Admin]} />,
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
