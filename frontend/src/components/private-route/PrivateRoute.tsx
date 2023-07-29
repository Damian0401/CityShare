import { Navigate } from "react-router-dom";
import { useStore } from "../../common/stores/store";
import { IPrivateRouteProps } from "./IPrivateRouteProps";
import { Routes } from "../../common/enums";

const PrivateRoute: React.FC<IPrivateRouteProps> = ({ Component, Roles }) => {
  const { authStore } = useStore();

  if (!authStore.user) {
    return <Navigate to={Routes.Login} />;
  }

  if (Roles && !Roles.some((r) => authStore.user?.roles?.includes(r))) {
    return <Navigate to={Routes.NotFound} />;
  }

  return <Component />;
};

export default PrivateRoute;
