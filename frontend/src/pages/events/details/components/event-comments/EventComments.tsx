import styles from "./EventComments.module.scss";
import BaseContainer from "../../../../../components/base-container/BaseContainer";
import { Containers } from "../../../../../common/enums";
import { Input, InputGroup, InputRightElement } from "@chakra-ui/react";
import { AiOutlineSend } from "react-icons/ai";
import { formatDistanceToNow } from "date-fns";
import LoadingSpinner from "../../../../../components/loading-spinner/LoadingSpinner";
import { useStore } from "../../../../../common/stores/store";
import { observer } from "mobx-react-lite";
import { useRef } from "react";

const EventComments: React.FC = observer(() => {
  const { authStore, eventStore } = useStore();

  const inputRef = useRef<HTMLInputElement>(null);

  const handleAddComment = async () => {
    const message = inputRef.current?.value;

    if (!message || !authStore.user || !eventStore.selectedEvent) return;

    await eventStore.addComment(eventStore.selectedEvent.id, message);

    inputRef.current.value = "";
  };

  if (!eventStore.comments)
    return (
      <div className={styles.spinner}>
        <LoadingSpinner />
      </div>
    );

  return (
    <div className={styles.container}>
      <div className={styles.title}>Comments:</div>
      <InputGroup>
        <InputRightElement
          children={<AiOutlineSend />}
          onClick={handleAddComment}
        />
        <Input ref={inputRef} placeholder="Add a comment" />
      </InputGroup>
      <div className={styles.comments}>
        {eventStore.comments.map((comment) => (
          <BaseContainer
            key={comment.id}
            type={Containers.Secondary}
            className={styles.comment}
          >
            <div className={styles.header}>
              <span>{comment.author}</span>
              <span>{formatDistanceToNow(comment.createdAt)}</span>
            </div>
            <div>{comment.message}</div>
          </BaseContainer>
        ))}
      </div>
    </div>
  );
});

export default EventComments;
