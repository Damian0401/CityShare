import {
  AlertDialog,
  AlertDialogBody,
  AlertDialogCloseButton,
  AlertDialogContent,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogOverlay,
  Button,
} from "@chakra-ui/react";
import { useRef } from "react";
import { IConfirmDialogProps } from "./IConfirmDialogProps";
import styles from "./ConfirmDialog.module.scss";

const ConfirmDialog: React.FC<IConfirmDialogProps> = (props) => {
  const { isOpen, header, body, onClose, onConfirm } = props;
  const cancelRef = useRef<HTMLButtonElement>(null);

  return (
    <>
      <AlertDialog
        motionPreset="slideInBottom"
        leastDestructiveRef={cancelRef}
        onClose={onClose}
        isOpen={isOpen}
        isCentered
      >
        <AlertDialogOverlay />
        <AlertDialogContent>
          <AlertDialogHeader>{header}</AlertDialogHeader>
          <AlertDialogBody>{body}</AlertDialogBody>
          <AlertDialogCloseButton />
          <AlertDialogFooter className={styles.footer}>
            <Button onClick={onConfirm}>Yes</Button>
            <Button ref={cancelRef} onClick={onClose}>
              No
            </Button>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </>
  );
};

export default ConfirmDialog;
