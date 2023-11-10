import { Navigate } from "react-router-dom";
import { Routes } from "../../common/enums";

const Home = () => {
  return <Navigate to={Routes.EventsMap} />;
};

export default Home;
