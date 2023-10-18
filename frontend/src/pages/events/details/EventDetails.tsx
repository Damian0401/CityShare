import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Containers, Routes, StatusCodes } from "../../../common/enums";
import styles from "./EventDetails.module.scss";
import BaseContainer from "../../../components/base-container/BaseContainer";
import EventImages from "./components/event-images/EventImages";
import EventBody from "./components/event-body/EventBody";
import EventMap from "./components/event-map/EventMap";
import { useStore } from "../../../common/stores/store";
import { AxiosError } from "axios";
import EventComments from "./components/event-comments/EventComments";

const EventDetails = observer(() => {
  const { id } = useParams<{ id: string }>();

  const { commonStore, eventStore } = useStore();

  const navigate = useNavigate();

  const [commentsVisible, setCommentsVisible] = useState(false);

  useEffect(() => {
    if (!id) {
      navigate(Routes.NotFound);
      return;
    }

    commonStore.setLoading(true);

    const controller = new AbortController();
    const loadEvent = async () => {
      try {
        await eventStore.loadSelectedEvent(id, controller.signal);
        commonStore.setLoading(false);
      } catch (error) {
        if (
          error instanceof AxiosError &&
          error.response?.status === StatusCodes.NotFound
        ) {
          navigate(Routes.NotFound);
          return;
        }
      }
    };

    loadEvent();

    return () => controller.abort();
  }, [id, commonStore, eventStore, navigate]);

  const handleLikeClick = async (id: string) => {
    await eventStore.updateLikes(id);
  };

  const handleCommentsClick = () => {
    if (!commentsVisible) {
      eventStore.createHubConnection();
    } else {
      eventStore.stopHubConnection();
    }
    setCommentsVisible(!commentsVisible);
  };

  return (
    <>
      {eventStore.selectedEvent && (
        <>
          <BaseContainer
            type={Containers.Tertiary}
            className={styles.container}
          >
            <BaseContainer type={Containers.Tertiary} className={styles.event}>
              <EventImages imageUrls={eventStore.selectedEvent.imageUrls} />
              <EventMap address={eventStore.selectedEvent.address} />
              <EventBody
                event={eventStore.selectedEvent}
                onLikeClick={handleLikeClick}
                onCommentClick={handleCommentsClick}
              />
            </BaseContainer>
            {commentsVisible && <EventComments />}
          </BaseContainer>
        </>
      )}
    </>
  );
});

export default EventDetails;
