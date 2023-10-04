import { Containers, Routes } from "../../../../../common/enums";
import BaseContainer from "../../../../../components/base-container/BaseContainer";
import { ISearchResultProps } from "./ISearchResultProps";
import styles from "./SearchResult.module.scss";
import { observer } from "mobx-react-lite";
import { IEvent } from "../../../../../common/interfaces";
import { useNavigate } from "react-router-dom";
import Category from "../../../../../components/categories/Categories";
import { formatDistanceToNow } from "date-fns";
import LikeButton from "../../../../../components/like-button/LikeButton";
import { useStore } from "../../../../../common/stores/store";
import Constants from "../../../../../common/utils/constants";

const SearchResult: React.FC<ISearchResultProps> = observer((props) => {
  const { events, onLikeClick } = props;

  const { eventStore } = useStore();

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

  const handleEventClick = (eventId: string) => {
    const selectedEvent = events.find((e) => e.id === eventId);

    if (!selectedEvent) return;

    eventStore.setSelectedEvent(selectedEvent);
    navigate(Routes.Events + "/" + eventId);
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
                    : Constants.ImagePlaceholder
                }
                alt="event"
              />
            </div>
            <div className={styles.header}>
              <div>
                <span className={styles.title}>{event.title}</span>
                <Category categoryIds={event.categoryIds} />
              </div>
              <LikeButton
                id={event.id}
                likes={event.likes}
                isLiked={event.isLiked}
                onLike={onLikeClick}
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
