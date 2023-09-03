import { Containers, Routes } from "../../../../../common/enums";
import BaseContainer from "../../../../../components/base-container/BaseContainer";
import { ISearchResultProps } from "./ISearchResultProps";
import styles from "./SearchResult.module.scss";
import { observer } from "mobx-react-lite";
import { IEvent } from "../../../../../common/interfaces";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import Category from "../../../../../components/categories/Categories";
import { formatDistanceToNow } from "date-fns";
import LikeButtons from "../../../../../components/like-buttons/LikeButtons";

const SearchResult: React.FC<ISearchResultProps> = observer((props) => {
  const { events } = props;

  const navigate = useNavigate();

  const getEventStatus = (event: IEvent) => {
    const currentDate = new Date();
    if (event.endDate < currentDate) {
      return "Finished";
    }

    if (event.startDate < currentDate) {
      return "Started";
    }

    return "Not started";
  };

  const handleEventClick = (eventId: number) => {
    navigate(Routes.Events + "/" + eventId);
  };

  const handleLikeClick = (eventId: number, isLiked: boolean) => {
    if (isLiked) {
      toast.success(`Event ${eventId} likes increased`);
      return;
    }

    toast.error(`Event ${eventId} likes decreased`);
  };

  return (
    <div className={styles.container}>
      {events.length === 0 ? (
        <p>No events found</p>
      ) : (
        events.map((event) => (
          <BaseContainer
            type={Containers.Tertiary}
            className={styles.event}
            key={event.id}
            onClick={() => handleEventClick(event.id)}
          >
            <div className={styles.image}>
              <img
                src={
                  event.imageUrls.length > 0
                    ? event.imageUrls[0]
                    : "https://picsum.photos/200/300"
                }
                alt="event"
              />
            </div>
            <div className={styles.header}>
              <div>
                <span className={styles.title}>{event.title}</span>
                <Category categoryIds={event.categoryIds} />
              </div>
              <LikeButtons
                id={event.id}
                likes={event.likes}
                isLiked={event.isLiked}
                onLike={handleLikeClick}
              />
            </div>
            <div className={styles.body}>
              <div>{getEventStatus(event)}</div>
              <div>{event.description}</div>
            </div>
            <div className={styles.footer}>
              <div>
                <div>{formatDistanceToNow(event.createdAt)} ago</div>
              </div>
              <div>{event.author}</div>
            </div>
          </BaseContainer>
        ))
      )}
    </div>
  );
});

export default SearchResult;
