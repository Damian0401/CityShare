import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Containers, Routes } from "../../../common/enums";
import styles from "./EventDetails.module.scss";
import BaseContainer from "../../../components/base-container/BaseContainer";
import EventImages from "./components/event-images/EventImages";
import EventBody from "./components/event-body/EventBody";
import EventMap from "./components/event-map/EventMap";
import { useStore } from "../../../common/stores/store";

const EventDetails = observer(() => {
  const { id } = useParams<{ id: string }>();

  const { commonStore, eventStore } = useStore();

  const navigate = useNavigate();

  useEffect(() => {
    if (!id) {
      navigate(Routes.NotFound);
      return;
    }

    commonStore.setLoading(true);

    const loadEvent = async () => {
      await eventStore.loadSelectedEvent(id);
      commonStore.setLoading(false);
    };

    loadEvent();
  }, [id, commonStore, eventStore, navigate]);

  return (
    <div className={styles.container}>
      {eventStore.selectedEvent && (
        <BaseContainer type={Containers.Tertiary} className={styles.event}>
          <EventImages imageUrls={eventStore.selectedEvent.imageUrls} />
          <EventMap address={eventStore.selectedEvent.address} />
          <EventBody
            event={eventStore.selectedEvent}
            onLikeClick={() => console.log("liked")}
          />
        </BaseContainer>
      )}
      <div className={styles.comments}>
        <BaseContainer type={Containers.Secondary}>Comment</BaseContainer>
      </div>
    </div>
  );
});

export default EventDetails;
