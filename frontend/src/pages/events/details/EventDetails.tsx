import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Containers, Routes } from "../../../common/enums";
import styles from "./EventDetails.module.scss";
import { IEvent } from "../../../common/interfaces";
import LoadingSpinner from "../../../components/loading-spinner/LoadingSpinner";
import BaseContainer from "../../../components/base-container/BaseContainer";
import EventImages from "./components/event-images/EventImages";
import EventBody from "./components/event-body/EventBody";
import EventMap from "./components/event-map/EventMap";
import { updateLikes } from "../../../common/utils/helpers";

const events: IEvent[] = [
  {
    id: "1",
    title: "Event 1",
    description:
      "Event 1 description, a little bit longer, ok maybe a little bit more longer, let's see how it will look like",
    cityId: 1,
    categoryIds: [1, 2, 3],
    imageUrls: [
      "https://picsum.photos/600/600",
      "https://picsum.photos/600/650",
      "https://picsum.photos/650/600",
      "https://picsum.photos/600/550",
      "https://picsum.photos/550/600",
    ],
    address: {
      point: { x: 51.1059776, y: 17.0356689 },
      displayName:
        "Address 1 a little bit longer, ok maybe a little bit more longer",
    },
    startDate: new Date(),
    endDate: new Date(),
    createdAt: new Date(),
    likes: 5,
    author: "Author 1",
    commentNumber: 10,
  },
  {
    id: "2",
    title: "Event 2",
    description: "Event 2 description",
    cityId: 1,
    categoryIds: [1, 2, 4],
    imageUrls: [],
    address: {
      point: { x: 51.1109776, y: 17.0306689 },
      displayName: "Address 2",
    },
    startDate: new Date(),
    endDate: new Date(),
    createdAt: new Date(),
    likes: 10,
    author: "Author 2",
    commentNumber: 10,
  },
];

const EventDetails = observer(() => {
  const { id } = useParams<{ id: string }>();

  const navigate = useNavigate();

  const [event, setEvent] = useState<IEvent>();

  useEffect(() => {
    if (!id) {
      navigate(Routes.NotFound);
      return;
    }

    const event = events.find((p) => p.id === id);

    if (!event) {
      navigate(Routes.NotFound);
      return;
    }

    setEvent(event);
  }, [id, navigate]);

  const handleLikeClick = (_: number, isLiked: boolean) => {
    if (!event) return;

    updateLikes(event, isLiked);

    setEvent({ ...event });
  };

  if (!event) return <LoadingSpinner />;

  return (
    <div className={styles.container}>
      <BaseContainer type={Containers.Tertiary} className={styles.event}>
        <EventImages imageUrls={event.imageUrls} />
        <EventMap address={event.address} />
        <EventBody event={event} onLikeClick={handleLikeClick} />
      </BaseContainer>
      <div className={styles.comments}>
        <BaseContainer type={Containers.Secondary}>Comment</BaseContainer>
      </div>
    </div>
  );
});

export default EventDetails;
