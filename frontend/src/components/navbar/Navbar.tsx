import { Link } from "react-router-dom";
import styles from "./Navbar.module.scss";
import ToggleThemeButton from "../toggle-theme-button/ToggleThemeButton";
import Routes from "../../common/navigation/Routes";

const Navbar = () => {
  return (
    <div className={styles.container}>
      <div>
        <Link to={Routes.Index}>Home</Link>
        <Link to={Routes.Login}>Login</Link>
        <Link to={Routes.Register}>Register</Link>
      </div>
      <ToggleThemeButton />
    </div>
  );
};

export default Navbar;
