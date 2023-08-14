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
import AddressSearchMap from "../components/address-search-map/AddressSearchMap";
import { toast } from "react-toastify";
import ConfirmEmail from "./confirm-email/ConfirmEmail";
import { Marker, Popup } from "react-leaflet";

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
        path: Routes.Map,
        element: (
          <AddressSearchMap
            initialPoint={{ x: 51.1089776, y: 17.0326689 }}
            isSearchOnly={true}
            additionalQuery="Wrocław"
            elements={[
              <Marker position={[51.1089776, 17.0326689]}>
                <Popup>
                  A pretty CSS3 popup. <br /> Easily customizable.
                </Popup>
              </Marker>,
              <Marker position={[51.1109776, 17.0346689]}>
                <Popup>
                  A pretty CSS3 popup. <br /> Easily customizable.
                </Popup>
              </Marker>,
            ]}
            onSelect={(result) => toast.success(result.displayName)}
          />
        ),
      },
      {
        path: Routes.Wildcard,
        element: <Navigate to={Routes.NotFound} />,
      },
    ],
  },
]);

export default Router;
