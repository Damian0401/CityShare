import { Link } from "react-router-dom";
import styles from "./Navbar.module.scss";
import ToggleThemeButton from "../toggle-theme-button/ToggleThemeButton";
import { Containers, Routes } from "../../common/enums";
import BaseContainer from "../base-container/BaseContainer";
import NavbarLogo from "../../assets/images/navbar-logo.svg";
import { Spacer } from "@chakra-ui/react";

const Navbar = () => {
  return (
    <BaseContainer className={styles.container} type={Containers.Navbar}>
      <div className={styles.tabs}>
        <Link to={Routes.Index} className={styles.logo}>
          <img src={NavbarLogo} className={styles.logoImage} />
          CityShare
        </Link>
      </div>
      <div className={styles.tabs}>
        <Link to={Routes.Login}>Login</Link>
        <Link to={Routes.Register}>Register</Link>
        <ToggleThemeButton />
      </div>
    </BaseContainer>
  );
};

export default Navbar;
