import styles from "./EventComments.module.scss";
import { IComment } from "../../../../../common/interfaces";
import BaseContainer from "../../../../../components/base-container/BaseContainer";
import { Containers } from "../../../../../common/enums";
import { Input, InputGroup, InputRightElement } from "@chakra-ui/react";
import { AiOutlineSend } from "react-icons/ai";
import { formatDistanceToNow } from "date-fns";
import { useRef, useState } from "react";
import LoadingSpinner from "../../../../../components/loading-spinner/LoadingSpinner";
import { useStore } from "../../../../../common/stores/store";
import { observer } from "mobx-react-lite";

const commentsMock: IComment[] = [
  {
    id: "1",
    message: "Comment 1",
    author: "Author 1",
    createdAt: new Date(),
  },
  {
    id: "2",
    message: "Comment 2",
    author: "Author 2",
    createdAt: new Date(),
  },
  {
    id: "3",
    message: "Comment 3",
    author: "Author 3",
    createdAt: new Date(),
  },
];

const EventComments: React.FC = observer(() => {
  const { authStore, eventStore } = useStore();

  const [comments, setComments] = useState<IComment[]>(commentsMock);

  const inputRef = useRef<HTMLInputElement>(null);

  const handleAddComment = async () => {
    const message = inputRef.current?.value;

    if (!message || !authStore.user || !eventStore.selectedEvent) return;

    const newComment: IComment = {
      id: (comments.length + 1).toString(),
      message: message,
      author: authStore.user.userName,
      createdAt: new Date(),
    };

    await eventStore.addComment(eventStore.selectedEvent.id, newComment);

    setComments([...comments, newComment]);
    inputRef.current.value = "";
  };

  if (!comments)
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
        {comments.map((comment) => (
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
