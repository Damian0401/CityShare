import { getFormattedDate } from "../../../../common/utils/helpers";
import { ICollapsableRequestProps } from "./ICollapsableRequestProps";
import styles from "./CollapsableRequest.module.scss";
import { Button, Collapse, useDisclosure } from "@chakra-ui/react";
import { ChakraSizes, Containers, Routes } from "../../../../common/enums";
import BaseContainer from "../../../../components/base-container/BaseContainer";
import { Link } from "react-router-dom";

const CollapsableRequest: React.FC<ICollapsableRequestProps> = (props) => {
  const { request, onAccept, onReject } = props;
  const { isOpen, onToggle } = useDisclosure();

  const handleOnClickStopPropagation = (
    event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
    isAccepted: boolean
  ) => {
    event.stopPropagation();

    if (isAccepted) {
      onAccept(request.id);
      return;
    }

    onReject(request.id);
  };

  return (
    <BaseContainer type={Containers.Tertiary} className={styles.container}>
      <Collapse
        startingHeight={30}
        in={isOpen}
        className={styles.collapse}
        onClick={onToggle}
      >
        <div>{request.message}</div>
        <div className={styles.info}>
          <span>{request.author}</span>
          <span>{getFormattedDate(request.createdAt)}</span>
        </div>
        <div className={styles.footer}>
          <div>
            <Button
              size={ChakraSizes.Sm}
              as={Link}
              to={Routes.Events + "/" + request.eventId}
            >
              Go to event
            </Button>
          </div>
          <div className={styles.buttons}>
            <Button
              size={ChakraSizes.Sm}
              onClick={(e) => handleOnClickStopPropagation(e, true)}
            >
              Accept
            </Button>
            <Button
              size={ChakraSizes.Sm}
              onClick={(e) => handleOnClickStopPropagation(e, false)}
            >
              Reject
            </Button>
          </div>
        </div>
      </Collapse>
    </BaseContainer>
  );
};

export default CollapsableRequest;
