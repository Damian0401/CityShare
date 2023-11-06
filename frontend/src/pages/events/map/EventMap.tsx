import AddressSearchMap from "../../../components/address-search-map/AddressSearchMap";
import styles from "./EventMap.module.scss";
import { observer } from "mobx-react-lite";
import { useStore } from "../../../common/stores/store";
import { Checkbox, Select, Text } from "@chakra-ui/react";
import { ChakraSizes, StorageKeys } from "../../../common/enums";
import { useEffect, useState } from "react";
import { ICity, IEvent, IEventSearchQuery } from "../../../common/interfaces";
import { getSelectedCityId, updateLikes } from "../../../common/utils/helpers";
import MapMarker from "./components/MapMarker/MapMarker";
import { EventFilters } from "../../../common/enums/EventFilters";

const EventMap = observer(() => {
  const { commonStore, eventStore } = useStore();

  const [events, setEvents] = useState<IEvent[]>([]);

  const [selectedCity, setSelectedCity] = useState<ICity>(
    commonStore.cities.find((x) => x.id === getSelectedCityId()) ??
      commonStore.cities[0]
  );

  const [eventsToShow, setEventsToShow] = useState<IEvent[]>([]);

  const [selectedCategories, setSelectedCategories] = useState<boolean[]>(
    commonStore.categories.map(() => true)
  );

  useEffect(() => {
    commonStore.setLoading(true);

    const controller = new AbortController();
    const loadEvents = async () => {
      const query: IEventSearchQuery = {
        cityId: selectedCity.id,
        isNow: true,
        sortBy: EventFilters.MostPopular,
      };

      const events = await eventStore.getEventsByQuery(
        query,
        controller.signal
      );
      setEvents(events.content);
    };

    loadEvents();
    commonStore.setLoading(false);

    return () => controller.abort();
  }, [commonStore, eventStore, selectedCity.id]);

  useEffect(() => {
    const selectedCategoriesIds = selectedCategories
      .map((selected, index) =>
        selected ? commonStore.categories[index].id : -1
      )
      .filter((index) => index !== -1);

    setEventsToShow(
      events.filter((event) =>
        event.categoryIds.every((categoryId) =>
          selectedCategoriesIds.includes(categoryId)
        )
      )
    );
  }, [commonStore, events, selectedCategories]);

  const handleSelectCity = (index: number) => {
    setSelectedCity(commonStore.cities[index]);
    localStorage.setItem(
      StorageKeys.SelectedCityId,
      commonStore.cities[index].id.toString()
    );
  };

  const handleSelectCategories = (checked: boolean, index: number) => {
    setSelectedCategories([
      ...selectedCategories.slice(0, index),
      checked,
      ...selectedCategories.slice(index + 1),
    ]);
  };

  const handleLikeClick = async (eventId: string) => {
    await eventStore.updateLikes(eventId);

    const event = events.find((e) => e.id === eventId);

    if (!event) return;

    updateLikes(event);

    setEvents([...events]);
  };

  return (
    <div className={styles.container}>
      <div className={styles.map}>
        <AddressSearchMap
          searchInputSize={ChakraSizes.Md}
          initialPoint={selectedCity.address.point}
          additionalQuery={selectedCity.name}
          scrollToPoint={selectedCity.address.point}
          disableSelect
          elements={eventsToShow.map((event) => (
            <MapMarker
              event={event}
              key={event.id}
              onLikeClick={handleLikeClick}
            />
          ))}
        />
      </div>
      <div className={styles.content}>
        <div>
          <Text>City:</Text>
          <Select
            defaultValue={commonStore.cities.indexOf(selectedCity)}
            onChange={(event) => handleSelectCity(parseInt(event.target.value))}
          >
            {commonStore.cities.map((city, index) => (
              <option key={city.id} value={index}>
                {city.name}
              </option>
            ))}
          </Select>
        </div>
        <div>
          <Text>Categories</Text>
          <div className={styles.categories}>
            {commonStore.categories.map((category, index) => (
              <Checkbox
                key={category.id}
                isChecked={selectedCategories[index]}
                onChange={(e) =>
                  handleSelectCategories(e.target.checked, index)
                }
              >
                {category.name}
              </Checkbox>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
});

export default EventMap;
