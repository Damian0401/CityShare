import { IEventBodyProps } from "./IEventBodyProps";
import styles from "./EventBody.module.scss";
import Category from "../../../../../components/categories/Categories";
import LikeButtons from "../../../../../components/like-buttons/LikeButtons";
import { formatDistanceToNow } from "date-fns";
import { getFormattedDate } from "../../../../../common/utils/helpers";
import { MdOutlineComment } from "react-icons/md";
import { observer } from "mobx-react-lite";

const EventBody: React.FC<IEventBodyProps> = observer(({ event }) => {
  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <div className={styles.title}>
          <div className={styles.text}>
            <span>{event.title}</span>
            <Category categoryIds={event.categoryIds} />
          </div>
          <div className={styles.buttons}>
            <LikeButtons
              likes={event.likes}
              isLiked={event.isLiked}
              id={event.id}
              onLike={() => console.log("liked")}
              className={styles.likes}
            />
            <div className={styles.comment}>
              <MdOutlineComment /> {event.comments}
            </div>
          </div>
        </div>
        <div className={styles.info}>
          Published {formatDistanceToNow(event.createdAt)} ago by{" "}
          <span>{event.author}</span>
        </div>
      </div>
      <div className={styles.content}>{event.description}</div>
      <div className={styles.dates}>
        <div>Start date: {getFormattedDate(event.startDate)}</div>
        <div>End date: {getFormattedDate(event.endDate)}</div>
      </div>
    </div>
  );
});

export default EventBody;
