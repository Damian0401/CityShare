import { IAnonymousRouteProps } from "./IAnonymousRouteProps";
import { useStore } from "../../common/stores/store";
import { Navigate } from "react-router-dom";
import { Routes } from "../../common/enums";

const AnonymousRoute: React.FC<IAnonymousRouteProps> = ({ Component }) => {
  const { authStore } = useStore();

  if (authStore.user) {
    return <Navigate to={Routes.Index} />;
  }

  return <Component />;
};

export default AnonymousRoute;
