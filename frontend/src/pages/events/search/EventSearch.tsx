import styles from "./EventSearch.module.scss";
import { IEvent } from "../../../common/interfaces";
import SearchInput from "./components/search-input/SearchInput";
import SearchResult from "./components/search-result/SearchResult";
import { IEventSearchQuery } from "../../../common/interfaces/IEventSearchQuery";
import { observer } from "mobx-react-lite";
import { useState } from "react";
import { updateLikes } from "../../../common/utils/helpers";
import { useStore } from "../../../common/stores/store";

const EventSearch = observer(() => {
  const { eventStore } = useStore();

  const [events, setEvents] = useState<IEvent[]>([]);

  const handleSearch = async (query: IEventSearchQuery) => {
    const events = await eventStore.getEvents(query);
    setEvents(events.content);
  };

  const handleLikeClick = (eventId: string) => {
    const event = events.find((e) => e.id === eventId);

    if (!event) return;

    updateLikes(event);

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
