import { HamburgerIcon } from "@chakra-ui/icons";
import {
  Button,
  Divider,
  Modal,
  ModalBody,
  ModalContent,
  ModalHeader,
  ModalOverlay,
  Spacer,
  Text,
  Tooltip,
  useDisclosure,
} from "@chakra-ui/react";
import { IUserMenuProps } from "./IUserMenuProps";
import styles from "./UserMenu.module.scss";
import BaseContainer from "../../../base-container/BaseContainer";
import { Containers, MotionPresets, Routes } from "../../../../common/enums";
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

const UserMenu: React.FC<IUserMenuProps> = observer((props) => {
  const { user, logout } = props;
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
        <BaseContainer type={Containers.Primary} className={styles.wrapper}>
          <div onClick={onOpen} className={styles.menuButton}>
            <div className={styles.menuText}>
              <Text>Menu</Text>
            </div>
            <HamburgerIcon />
          </div>
        </BaseContainer>
      </div>
      <Modal
        onClose={onClose}
        isOpen={isOpen}
        initialFocusRef={initialRef}
        motionPreset={MotionPresets.SlideInRight}
      >
        <ModalOverlay />
        <ModalContent className={styles.menuModal}>
          <ModalHeader className={styles.header}>
            <Link to={Routes.Index} className={styles.logo}>
              <img src={NavbarLogo} className={styles.logoImage} />
              CityShare
            </Link>
            <Divider className={styles.divider} />
            <div className={styles.userName}>{user.userName}</div>
            {!user.emailConfirmed && (
              <Tooltip
                label="Confirm your email by clicking on the link from the email we sent you."
                aria-label={Constants.AriaLabels.ConfirmEmailTooltip}
              >
                <div className={styles.emailNotConfirmed}>
                  Email not confirmed <AiOutlineQuestionCircle />
                </div>
              </Tooltip>
            )}
          </ModalHeader>
          <ModalBody className={styles.modalBody}>
            <Button leftIcon={<CgProfile />} onClick={onClose} ref={initialRef}>
              Profile
            </Button>
            <Button
              leftIcon={<IoCreateOutline />}
              onClick={redirectAndClose(Routes.PostsCreate)}
              disabled={!user.emailConfirmed}
            >
              Create
            </Button>
            <Button
              leftIcon={<FiMapPin />}
              onClick={redirectAndClose(Routes.PostsMap)}
            >
              Map
            </Button>
            <Button
              leftIcon={<AiOutlineSearch />}
              onClick={redirectAndClose(Routes.PostsSearch)}
            >
              Search
            </Button>
            <Spacer />
            <Button leftIcon={<BiLogOutCircle />} onClick={logout}>
              Logout
            </Button>
          </ModalBody>
        </ModalContent>
      </Modal>
    </>
  );
});

export default UserMenu;
