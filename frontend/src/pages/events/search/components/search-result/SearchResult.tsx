import { format } from "date-fns";
import { Containers, Routes } from "../../../../../common/enums";
import BaseContainer from "../../../../../components/base-container/BaseContainer";
import { ISearchResultProps } from "./ISearchResultProps";
import styles from "./SearchResult.module.scss";
import { observer } from "mobx-react-lite";
import { useStore } from "../../../../../common/stores/store";
import { IEvent } from "../../../../../common/interfaces";
import {
  AiFillMinusSquare,
  AiFillPlusSquare,
  AiOutlineMinusSquare,
  AiOutlinePlusSquare,
} from "react-icons/ai";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import { MouseEvent } from "react";

const SearchResult: React.FC<ISearchResultProps> = observer((props) => {
  const { events } = props;

  const { commonStore } = useStore();

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

  const handleScoreClick = (
    eventId: number,
    isLiked: boolean,
    event: MouseEvent<HTMLSpanElement>
  ) => {
    event.stopPropagation();

    if (isLiked) {
      toast.success(`Event ${eventId} score increased`);
      return;
    }

    toast.error(`Event ${eventId} score decreased`);
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
                <span className={styles.title}>{event.title}</span>{" "}
                {event.categoryIds.map((id) => (
                  <span key={id} className={styles.category}>
                    {commonStore.categories.find((x) => x.id === id)?.name}
                  </span>
                ))}
              </div>
              <div className={styles.score}>
                <span
                  className={styles.icon}
                  onClick={(e) => handleScoreClick(event.id, true, e)}
                >
                  {event.isLiked ? (
                    <AiFillPlusSquare />
                  ) : (
                    <AiOutlinePlusSquare />
                  )}
                </span>
                <span>{event.score}</span>
                <span
                  className={styles.icon}
                  onClick={(e) => handleScoreClick(event.id, false, e)}
                >
                  {event?.isLiked === false ? (
                    <AiFillMinusSquare />
                  ) : (
                    <AiOutlineMinusSquare />
                  )}
                </span>
              </div>
            </div>
            <div className={styles.body}>
              <div>{getEventStatus(event)}</div>
              <div>{event.description}</div>
            </div>
            <div className={styles.footer}>
              <div>
                <div>
                  {format(new Date(event.createdAt), "dd/MM/yyyy HH:mm")}
                </div>
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
