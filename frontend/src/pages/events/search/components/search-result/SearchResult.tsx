import { Containers, Routes } from "../../../../../common/enums";
import BaseContainer from "../../../../../components/base-container/BaseContainer";
import { ISearchResultProps } from "./ISearchResultProps";
import styles from "./SearchResult.module.scss";
import { observer } from "mobx-react-lite";
import { useNavigate } from "react-router-dom";
import Categories from "../../../../../components/categories/Categories";
import { formatDistanceToNow } from "date-fns";
import LikeButton from "../../../../../components/like-button/LikeButton";
import { useStore } from "../../../../../common/stores/store";
import Constants from "../../../../../common/utils/constants";
import { getFormattedDate } from "../../../../../common/utils/helpers";

const SearchResult: React.FC<ISearchResultProps> = observer((props) => {
  const { events, onLikeClick } = props;

  const { eventStore } = useStore();

  const navigate = useNavigate();

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
                  event.images.length > 0
                    ? event.images[0].uri ?? Constants.Images.Urls.Processing
                    : Constants.Images.Urls.Placeholder
                }
                alt={Constants.Images.Alts.Event}
              />
            </div>
            <div className={styles.header}>
              <div>
                <span className={styles.title}>{event.title}</span>
                <Categories categoryIds={event.categoryIds} />
              </div>
              <LikeButton
                id={event.id}
                likes={event.likes}
                isLiked={event.isLiked}
                onLike={onLikeClick}
              />
            </div>
            <div className={styles.body}>
              <div>
                {getFormattedDate(event.startDate)} -{" "}
                {getFormattedDate(event.endDate)}
              </div>
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
