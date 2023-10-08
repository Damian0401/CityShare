import { Link, useNavigate } from "react-router-dom";
import styles from "./Navbar.module.scss";
import ToggleThemeButton from "../toggle-theme-button/ToggleThemeButton";
import { Containers, Routes } from "../../common/enums";
import BaseContainer from "../base-container/BaseContainer";
import NavbarLogo from "../../assets/images/navbar-logo.svg";
import { useStore } from "../../common/stores/store";
import { observer } from "mobx-react-lite";
import UserMenu from "./components/user-menu/UserMenu";

const Navbar = observer(() => {
  const { authStore } = useStore();

  const navigate = useNavigate();

  const handleLogout = async () => {
    await authStore.logout();
    navigate(Routes.Login);
  };

  return (
    <nav className={styles.container}>
      <BaseContainer className={styles.navbar} type={Containers.Navbar}>
        <div className={styles.tabs}>
          <Link to={Routes.Index} className={styles.logo}>
            <img src={NavbarLogo} className={styles.logoImage} />
            CityShare
          </Link>
        </div>
        <div className={styles.tabs}>
          {authStore.user ? (
            <>
              <Link to={Routes.Index}>Home</Link>
              <Link to={Routes.EventsMap}>Map</Link>
              <Link to={Routes.EventsSearch}>Search</Link>
            </>
          ) : (
            <>
              <Link to={Routes.Login}>Login</Link>
              <Link to={Routes.Register}>Register</Link>
            </>
          )}
          <ToggleThemeButton />
        </div>
      </BaseContainer>
      {authStore.user && (
        <UserMenu user={authStore.user} logout={handleLogout} />
      )}
    </nav>
  );
});

export default Navbar;
