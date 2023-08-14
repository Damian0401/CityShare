import { HamburgerIcon } from "@chakra-ui/icons";
import {
  Button,
  Modal,
  ModalBody,
  ModalContent,
  ModalHeader,
  ModalOverlay,
  Text,
  useDisclosure,
} from "@chakra-ui/react";
import { IUserMenuProps } from "./IUserMenuProps";
import styles from "./UserMenu.module.scss";
import BaseContainer from "../../../base-container/BaseContainer";
import { Containers, MotionPresets } from "../../../../common/enums";

const UserMenu: React.FC<IUserMenuProps> = (props) => {
  const { user, logout } = props;
  const { isOpen, onOpen, onClose } = useDisclosure();
  return (
    <>
      <div className={styles.container}>
        <BaseContainer type={Containers.Primary} className={styles.wrapper}>
          <div onClick={onOpen} className={styles.menuButton}>
            <div className={styles.userName}>
              <Text>Menu</Text>
            </div>
            <HamburgerIcon />
          </div>
        </BaseContainer>
      </div>
      <Modal
        onClose={onClose}
        isOpen={isOpen}
        motionPreset={MotionPresets.SlideInRight}
      >
        <ModalOverlay />
        <ModalContent className={styles.menuModal}>
          <ModalHeader>{user.userName}</ModalHeader>
          <ModalBody>
            <Button onClick={logout}>Logout</Button>
          </ModalBody>
        </ModalContent>
      </Modal>
    </>
  );
};

export default UserMenu;
