import styles from "./EventSearch.module.scss";
import { IEvent } from "../../../common/interfaces";
import SearchInput from "./components/search-input/SearchInput";
import SearchResult from "./components/search-result/SearchResult";
import { IEventSearchQuery } from "../../../common/interfaces/IEventSearchQuery";
import { observer } from "mobx-react-lite";
import { useState } from "react";
import { updateLikes } from "../../../common/utils/helpers";

const mockEvents: IEvent[] = [
  {
    id: 1,
    title: "Title 1",
    description: "Description 1",
    createdAt: new Date(),
    startDate: new Date("2021-10-10"),
    endDate: new Date("2022-10-10"),
    address: {
      displayName: "Address 1",
      point: {
        x: 1,
        y: 1,
      },
    },
    categoryIds: [1, 2],
    cityId: 1,
    imageUrls: ["https://picsum.photos/200/300"],
    likes: 5,
    author: "Author 1",
    isLiked: true,
    comments: 10,
  },
  {
    id: 2,
    title: "Title 2",
    description: "Description 2",
    createdAt: new Date(),
    startDate: new Date("2023-06-07"),
    endDate: new Date("2023-10-10"),
    address: {
      displayName: "Address 2",
      point: {
        x: 2,
        y: 2,
      },
    },
    categoryIds: [1, 2],
    cityId: 1,
    imageUrls: ["https://picsum.photos/200/300"],
    likes: 5,
    author: "Author 2",
    isLiked: false,
    comments: 10,
  },
  {
    id: 3,
    title: "Title 3",
    description: "Description 3",
    createdAt: new Date(),
    startDate: new Date("2023-10-10"),
    endDate: new Date("2024-10-10"),
    address: {
      displayName: "Address 3",
      point: {
        x: 3,
        y: 3,
      },
    },
    categoryIds: [1, 2],
    cityId: 1,
    imageUrls: ["https://picsum.photos/200/300"],
    likes: 5,
    author: "Author 3",
    comments: 10,
  },
];

const EventSearch = observer(() => {
  const [events, setEvents] = useState<IEvent[]>(mockEvents);

  const handleSearch = (query: IEventSearchQuery) => {
    console.log(query);
  };

  const handleLikeClick = (eventId: number, isLiked: boolean) => {
    const event = events.find((e) => e.id === eventId);

    if (!event) return;

    updateLikes(event, isLiked);

    setEvents([...events]);
  };

  return (
    <div className={styles.container}>
      <SearchInput onSearch={handleSearch} />
      <SearchResult events={events} onLikeClick={handleLikeClick} />
    </div>
  );
});

export default EventSearch;
