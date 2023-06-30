import { Link } from "react-router-dom";
import styles from "./Navbar.module.scss";
import { Routes } from "../../common/enums";

const Navbar = () => {
  return (
    <div className={styles.container}>
      <Link to={Routes.Index}>Home</Link>
      <Link to={Routes.Login}>Login</Link>
      <Link to={Routes.Register}>Register</Link>
    </div>
  );
};

export default Navbar;
