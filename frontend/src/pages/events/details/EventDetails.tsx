import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Containers, Routes } from "../../../common/enums";
import styles from "./EventDetails.module.scss";
import BaseContainer from "../../../components/base-container/BaseContainer";
import { IEvent } from "../../../common/interfaces";
import LoadingSpinner from "../../../components/loading-spinner/LoadingSpinner";

const events: IEvent[] = [
  {
    id: 1,
    title: "Event 1",
    description: "Event 1 description",
    cityId: 1,
    categoryIds: [1, 2, 3],
    imageUrls: [],
    address: {
      point: { x: 51.1059776, y: 17.0356689 },
      displayName: "Address 1",
    },
    startDate: new Date(),
    endDate: new Date(),
    createdAt: new Date(),
    score: 5,
    author: "Author 1",
  },
  {
    id: 2,
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
    score: 10,
    author: "Author 2",
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

    const event = events.find((p) => p.id === +id);

    if (!event) {
      navigate(Routes.NotFound);
      return;
    }

    setEvent(event);
  }, [id, navigate]);

  if (!event) return <LoadingSpinner />;

  return (
    <div className={styles.container}>
      <BaseContainer type={Containers.Primary} className={styles.event}>
        Details of event with id: {event.id}
      </BaseContainer>
      <div className={styles.comments}>Comments</div>
    </div>
  );
});

export default EventDetails;
