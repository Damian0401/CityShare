import AddressSearchMap from "../../../components/address-search-map/AddressSearchMap";
import styles from "./EventMap.module.scss";
import { observer } from "mobx-react-lite";
import { useStore } from "../../../common/stores/store";
import { Checkbox, Select, Text } from "@chakra-ui/react";
import { ChakraSizes } from "../../../common/enums";
import { useEffect, useState } from "react";
import { ICity, IEvent } from "../../../common/interfaces";
import { updateLikes } from "../../../common/utils/helpers";
import MapMarker from "./components/MapMarker/MapMarker";

const mockEvents: IEvent[] = [
  {
    id: 1,
    title: "Event 1",
    description:
      "Event 1 description, a little bit longer, ok maybe a little bit more longer, let's see how it will look like",
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
    likes: 5,
    author: "Author 1",
    comments: 10,
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
    likes: 10,
    author: "Author 2",
    comments: 20,
  },
  {
    id: 3,
    title: "Event 3",
    description: "Event 3 description",
    cityId: 1,
    categoryIds: [3],
    imageUrls: [],
    address: {
      point: { x: 51.1089776, y: 17.0326689 },
      displayName: "Address 3",
    },
    startDate: new Date(),
    endDate: new Date(),
    createdAt: new Date(),
    likes: 15,
    author: "Author 3",
    comments: 30,
  },
  {
    id: 4,
    title: "Event 4",
    description: "Event 4 description",
    cityId: 2,
    categoryIds: [1],
    imageUrls: [],
    address: {
      point: { x: 52.2349581, y: 21.0087249 },
      displayName: "Address 4",
    },
    startDate: new Date(),
    endDate: new Date(),
    createdAt: new Date(),
    likes: 20,
    author: "Author 4",
    comments: 40,
  },
  {
    id: 5,
    title: "Event 5",
    description: "Event 5 description",
    cityId: 2,
    categoryIds: [2, 3],
    imageUrls: [],
    address: {
      point: { x: 52.2319581, y: 21.0067249 },
      displayName: "Address 5",
    },
    startDate: new Date(),
    endDate: new Date(),
    createdAt: new Date(),
    likes: 25,
    author: "Author 5",
    comments: 50,
  },
];

const EventMap = observer(() => {
  const { commonStore } = useStore();

  const [events, setEvents] = useState<IEvent[]>(mockEvents);

  const [selectedCity, setSelectedCity] = useState<ICity>(
    commonStore.cities[0]
  );

  const [eventsToShow, setEventsToShow] = useState<IEvent[]>([]);

  const [selectedCategories, setSelectedCategories] = useState<boolean[]>(
    commonStore.categories.map(() => true)
  );

  useEffect(() => {
    const selectedCategoriesIds = selectedCategories
      .map((selected, index) =>
        selected ? commonStore.categories[index].id : -1
      )
      .filter((index) => index !== -1);

    setEventsToShow(
      events.filter(
        (event) =>
          event.cityId === selectedCity.id &&
          event.categoryIds.every((categoryId) =>
            selectedCategoriesIds.includes(categoryId)
          )
      )
    );
  }, [commonStore, events, selectedCity, selectedCategories]);

  const handleSelectCity = (index: number) => {
    setSelectedCity(commonStore.cities[index]);
  };

  const handleSelectCategories = (checked: boolean, index: number) => {
    setSelectedCategories([
      ...selectedCategories.slice(0, index),
      checked,
      ...selectedCategories.slice(index + 1),
    ]);
  };

  const handleLikeClick = (eventId: number, isLiked: boolean) => {
    const event = events.find((e) => e.id === eventId);

    if (!event) return;

    updateLikes(event, isLiked);

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
            defaultValue={0}
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
