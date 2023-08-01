import { Link } from "react-router-dom";
import styles from "./Navbar.module.scss";
import ToggleThemeButton from "../toggle-theme-button/ToggleThemeButton";
import { Containers, Routes } from "../../common/enums";
import BaseContainer from "../base-container/BaseContainer";
import NavbarLogo from "../../assets/images/navbar-logo.svg";
import { useStore } from "../../common/stores/store";
import { observer } from "mobx-react-lite";
import UserMenu from "./components/user-menu/UserMenu";
import Router from "../../pages/Router";

const Navbar = observer(() => {
  const { authStore } = useStore();

  const handleLogout = async () => {
    await authStore.logout();
    Router.navigate(Routes.Login);
  };

  return (
    <div className={styles.container}>
      <BaseContainer className={styles.navbar} type={Containers.Navbar}>
        <div className={styles.tabs}>
          <Link to={Routes.Index} className={styles.logo}>
            <img src={NavbarLogo} className={styles.logoImage} />
            CityShare
          </Link>
        </div>
        <div className={styles.tabs}>
          <Link to={Routes.Test}>Test</Link>
          <Link to={Routes.Login}>Login</Link>
          <Link to={Routes.Register}>Register</Link>
          <ToggleThemeButton />
        </div>
      </BaseContainer>
      {authStore.user && (
        <UserMenu user={authStore.user} logout={handleLogout} />
      )}
    </div>
  );
});

export default Navbar;
