import { HamburgerIcon } from "@chakra-ui/icons";
import {
  Button,
  Divider,
  Drawer,
  DrawerBody,
  DrawerContent,
  DrawerHeader,
  DrawerOverlay,
  Spacer,
  Tooltip,
  useDisclosure,
} from "@chakra-ui/react";
import { IUserMenuProps } from "./IUserMenuProps";
import styles from "./UserMenu.module.scss";
import { DrawerPlacements, Routes } from "../../../../common/enums";
import { BiLogOutCircle } from "react-icons/bi";
import { CgProfile } from "react-icons/cg";
import { IoCreateOutline } from "react-icons/io5";
import { AiOutlineSearch } from "react-icons/ai";
import { FiMapPin } from "react-icons/fi";
import { AiOutlineQuestionCircle } from "react-icons/ai";
import NavbarLogo from "../../../../assets/images/navbar-logo.svg";
import { Link, useNavigate } from "react-router-dom";
import { useRef } from "react";
import Constants from "../../../../common/utils/constants";
import { observer } from "mobx-react-lite";
import ToggleThemeButton from "./components/toggle-theme-button/ToggleThemeButton";
import { useStore } from "../../../../common/stores/store";
import { MdOutlineAdminPanelSettings } from "react-icons/md";

const UserMenu: React.FC<IUserMenuProps> = observer((props) => {
  const { logout } = props;
  const {
    authStore: { user, isAdmin },
  } = useStore();
  const { isOpen, onOpen, onClose } = useDisclosure();
  const initialRef = useRef<HTMLButtonElement | null>(null);
  const navigate = useNavigate();
  const redirectAndClose = (path: string) => {
    return () => {
      navigate(path);
      onClose();
    };
  };
  return (
    <>
      <div className={styles.container}>
        <HamburgerIcon
          _hover={{}}
          onClick={onOpen}
          className={styles.menuButton}
        />
      </div>
      <Drawer
        onClose={onClose}
        isOpen={isOpen}
        initialFocusRef={initialRef}
        placement={DrawerPlacements.Right}
      >
        <DrawerOverlay />
        <DrawerContent className={styles.menuModal}>
          <DrawerHeader className={styles.header}>
            <Link to={Routes.Index} className={styles.logo}>
              <img src={NavbarLogo} className={styles.logoImage} />
              CityShare
            </Link>
            <Divider className={styles.divider} />
            <div className={styles.userName}>{user?.userName || ""}</div>
            {!user?.emailConfirmed && (
              <Tooltip
                label="Confirm your email by clicking on the link from the email we sent you."
                aria-label={Constants.AriaLabels.ConfirmEmailTooltip}
              >
                <div className={styles.emailNotConfirmed}>
                  Email not confirmed <AiOutlineQuestionCircle />
                </div>
              </Tooltip>
            )}
          </DrawerHeader>
          <DrawerBody className={styles.modalBody}>
            <ToggleThemeButton buttonRef={initialRef} />
            {isAdmin && (
              <Button
                leftIcon={<MdOutlineAdminPanelSettings />}
                onClick={redirectAndClose(Routes.Requsts)}
              >
                Requsts
              </Button>
            )}
            <Button leftIcon={<CgProfile />} onClick={onClose}>
              Profile
            </Button>
            <Button
              leftIcon={<AiOutlineSearch />}
              onClick={redirectAndClose(Routes.EventsSearch)}
            >
              Search
            </Button>
            <Button
              leftIcon={<FiMapPin />}
              onClick={redirectAndClose(Routes.EventsMap)}
            >
              Map
            </Button>
            <Button
              leftIcon={<IoCreateOutline />}
              onClick={redirectAndClose(Routes.EventsCreate)}
              isDisabled={!user?.emailConfirmed}
            >
              Create
            </Button>
            <Spacer />
            <Button leftIcon={<BiLogOutCircle />} onClick={logout}>
              Logout
            </Button>
          </DrawerBody>
        </DrawerContent>
      </Drawer>
    </>
  );
});

export default UserMenu;
