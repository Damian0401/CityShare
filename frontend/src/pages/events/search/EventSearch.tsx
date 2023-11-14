import styles from "./EventSearch.module.scss";
import { IEvent } from "../../../common/interfaces";
import SearchInput from "./components/search-input/SearchInput";
import SearchResult from "./components/search-result/SearchResult";
import { IEventSearchQuery } from "../../../common/interfaces/IEventSearchQuery";
import { observer } from "mobx-react-lite";
import { useState } from "react";
import { updateLikes } from "../../../common/utils/helpers";
import { useStore } from "../../../common/stores/store";
import { IconButton } from "@chakra-ui/react";
import { IoIosArrowDown } from "react-icons/io";
import Constants from "../../../common/utils/constants";

const EventSearch = observer(() => {
  const { eventStore } = useStore();

  const [events, setEvents] = useState<IEvent[]>([]);
  const [loadedPages, setLoadedPages] = useState<number>(0);
  const [totalPages, setTotalPages] = useState<number>(0);
  const [lastQuery, setLastQuery] = useState<IEventSearchQuery>();

  const handleSearch = async (query: IEventSearchQuery) => {
    const events = await eventStore.getEventsByQuery(query);
    setEvents(events.content);
    setLoadedPages(events.pageNumber);
    setTotalPages(events.totalPages);
    setLastQuery(query);
  };

  const handleLikeClick = async (eventId: string) => {
    await eventStore.updateLikes(eventId);

    const event = events.find((e) => e.id === eventId);

    if (!event) return;

    updateLikes(event);

    setEvents([...events]);
  };

  const handleLoadMore = async () => {
    if (!lastQuery) return;

    const query = lastQuery;
    query.pageNumber = loadedPages + 1;
    const newEvents = await eventStore.getEventsByQuery(query);
    setEvents([...events, ...newEvents.content]);
    setLoadedPages(newEvents.pageNumber);
    setTotalPages(newEvents.totalPages);
  };

  return (
    <div className={styles.container}>
      <SearchInput onSearch={handleSearch} />
      <SearchResult events={events} onLikeClick={handleLikeClick} />
      {loadedPages < totalPages && (
        <IconButton
          aria-label={Constants.AriaLabels.LoadMore}
          icon={<IoIosArrowDown />}
          className={styles.load}
          onClick={handleLoadMore}
        />
      )}
    </div>
  );
});

export default EventSearch;
