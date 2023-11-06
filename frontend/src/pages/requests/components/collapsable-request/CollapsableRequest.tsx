import { getFormattedDate } from "../../../../common/utils/helpers";
import { ICollapsableRequestProps } from "./ICollapsableRequestProps";
import styles from "./CollapsableRequest.module.scss";
import { Button, Collapse, useDisclosure } from "@chakra-ui/react";
import {
  ChakraSizes,
  Containers,
  LinkTargets,
  Routes,
} from "../../../../common/enums";
import BaseContainer from "../../../../components/base-container/BaseContainer";
import { Link } from "react-router-dom";
import Constants from "../../../../common/utils/constants";
import { useState } from "react";

const CollapsableRequest: React.FC<ICollapsableRequestProps> = (props) => {
  const { request, onAccept, onReject } = props;
  const { isOpen, onToggle } = useDisclosure();
  const [isAcceptLoading, setIsAcceptLoading] = useState(false);
  const [isRejectLoading, setIsRejectLoading] = useState(false);

  const handleOnClickStopPropagation = (
    event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
    isAccepted: boolean
  ) => {
    event.stopPropagation();

    if (isAccepted) {
      setIsAcceptLoading(true);
      onAccept(request.id);
      setIsAcceptLoading(false);
      return;
    }

    setIsRejectLoading(true);
    onReject(request.id);
    setIsRejectLoading(false);
  };

  return (
    <BaseContainer type={Containers.Tertiary} className={styles.container}>
      <Collapse
        startingHeight={Constants.StartingCollapseHeight}
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
              target={LinkTargets.Blank}
              to={
                Routes.Events + `/${request.eventId}?imageId=${request.imageId}`
              }
            >
              Go to event
            </Button>
          </div>
          <div className={styles.buttons}>
            <Button
              size={ChakraSizes.Sm}
              isLoading={isAcceptLoading}
              onClick={(e) => handleOnClickStopPropagation(e, true)}
            >
              Accept
            </Button>
            <Button
              size={ChakraSizes.Sm}
              isLoading={isRejectLoading}
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
