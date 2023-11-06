import {
  Button,
  Modal,
  ModalBody,
  ModalContent,
  ModalOverlay,
  Textarea,
  useDisclosure,
} from "@chakra-ui/react";
import { useStore } from "../../../../../../../common/stores/store";
import { IRequestsModalProps } from "./IRequestsModalProps";
import styles from "./RequestsModal.module.scss";
import Constants from "../../../../../../../common/utils/constants";
import { useRef, useState } from "react";
import { IRequestCreateValue } from "../../../../../../../common/interfaces";

const RequestsModal: React.FC<IRequestsModalProps> = (props) => {
  const {
    isOpen,
    imageId,
    imageUri,
    imageAlt = Constants.Images.Alts.Event,
    onClose,
  } = props;

  const {
    isOpen: isFormOpen,
    onOpen: onFormOpen,
    onClose: onFormClose,
  } = useDisclosure();

  const [selectedRequestTypeId, setSelectedRequestTypeId] = useState<number>();

  const [isFormLoading, setIsFormLoading] = useState<boolean>(false);

  const { commonStore, authStore, eventStore, requestStore } = useStore();

  const messageRef = useRef<HTMLTextAreaElement>(null);

  const handleRequestTypeClick = (requestTypeId: number) => {
    if (!selectedRequestTypeId) {
      setSelectedRequestTypeId(requestTypeId);
      onFormOpen();
      return;
    }

    if (selectedRequestTypeId === requestTypeId) {
      setSelectedRequestTypeId(undefined);
      onFormClose();
      return;
    }

    setSelectedRequestTypeId(requestTypeId);
  };

  const userCanSeeRequests = () => {
    if (authStore.isAdmin) {
      return false;
    }

    if (authStore.user?.userName === eventStore.selectedEvent?.author) {
      return false;
    }

    if (!authStore.user?.emailConfirmed) {
      return false;
    }

    return true;
  };

  const handleSendRequest = async () => {
    if (!selectedRequestTypeId || !messageRef.current) return;

    const message = messageRef.current.value;

    if (!message) return;

    const values: IRequestCreateValue = {
      imageId: imageId,
      typeId: selectedRequestTypeId,
      message,
    };

    setIsFormLoading(true);
    await requestStore.createRequest(values);
    setIsFormLoading(false);

    onFormClose();
    setSelectedRequestTypeId(undefined);
    messageRef.current.value = "";
  };

  return (
    <Modal onClose={onClose} isOpen={isOpen} isCentered>
      <ModalOverlay />
      <ModalContent>
        <ModalBody className={styles.body}>
          <img src={imageUri} alt={imageAlt} />
          {userCanSeeRequests() && (
            <div className={styles.requests}>
              <div>Create request:</div>
              <div className={styles.buttons}>
                {commonStore.requestTypes.map((requestType) => (
                  <Button
                    className={
                      styles.button +
                      " " +
                      (selectedRequestTypeId === requestType.id
                        ? styles.selected
                        : "")
                    }
                    key={requestType.id}
                    onClick={() => handleRequestTypeClick(requestType.id)}
                  >
                    {requestType.name}
                  </Button>
                ))}
              </div>
              {isFormOpen && (
                <div className={styles.form}>
                  <Textarea
                    ref={messageRef}
                    placeholder={Constants.Placeholders.RequestMessage}
                  />
                  <Button
                    isLoading={isFormLoading}
                    onClick={handleSendRequest}
                    className={styles.button}
                  >
                    Send
                  </Button>
                </div>
              )}
            </div>
          )}
        </ModalBody>
      </ModalContent>
    </Modal>
  );
};

export default RequestsModal;
