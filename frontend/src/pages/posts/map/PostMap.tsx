import AddressSearchMap from "../../../components/address-search-map/AddressSearchMap";
import styles from "./PostMap.module.scss";
import { observer } from "mobx-react-lite";
import { useStore } from "../../../common/stores/store";
import { Checkbox, Select, Text } from "@chakra-ui/react";
import { ChakraSizes, Routes } from "../../../common/enums";
import { useEffect, useState } from "react";
import { ICity, IPost } from "../../../common/interfaces";
import { Marker, Popup } from "react-leaflet";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";

const posts: IPost[] = [
  {
    id: 1,
    title: "Post 1",
    description: "Post 1 description",
    cityId: 1,
    categoryIds: [1, 2, 3],
    imageUrl: "",
    address: {
      id: 1,
      point: { x: 51.1059776, y: 17.0356689 },
      displayName: "Address 1",
      boundingBox: {
        maxX: 0,
        maxY: 0,
        minX: 0,
        minY: 0,
      },
    },
  },
  {
    id: 2,
    title: "Post 2",
    description: "Post 2 description",
    cityId: 1,
    categoryIds: [1, 2, 4],
    imageUrl: "",
    address: {
      id: 2,
      point: { x: 51.1109776, y: 17.0306689 },
      displayName: "Address 2",
      boundingBox: {
        maxX: 0,
        maxY: 0,
        minX: 0,
        minY: 0,
      },
    },
  },
  {
    id: 3,
    title: "Post 3",
    description: "Post 3 description",
    cityId: 1,
    categoryIds: [3],
    imageUrl: "",
    address: {
      id: 3,
      point: { x: 51.1089776, y: 17.0326689 },
      displayName: "Address 3",
      boundingBox: {
        maxX: 0,
        maxY: 0,
        minX: 0,
        minY: 0,
      },
    },
  },
  {
    id: 4,
    title: "Post 4",
    description: "Post 4 description",
    cityId: 2,
    categoryIds: [1],
    imageUrl: "",
    address: {
      id: 4,
      point: { x: 52.2349581, y: 21.0087249 },
      displayName: "Address 4",
      boundingBox: {
        maxX: 0,
        maxY: 0,
        minX: 0,
        minY: 0,
      },
    },
  },
  {
    id: 5,
    title: "Post 5",
    description: "Post 5 description",
    cityId: 2,
    categoryIds: [2, 3],
    imageUrl: "",
    address: {
      id: 5,
      point: { x: 52.2319581, y: 21.0067249 },
      displayName: "Address 5",
      boundingBox: {
        maxX: 0,
        maxY: 0,
        minX: 0,
        minY: 0,
      },
    },
  },
];

const PostMap = observer(() => {
  const { commonStore } = useStore();

  const navigate = useNavigate();

  const [selectedCity, setSelectedCity] = useState<ICity>(
    commonStore.cities[0]
  );

  const [postsToShow, setPostsToShow] = useState<IPost[]>(
    posts.filter((post) => post.cityId === selectedCity.id)
  );

  const [selectedCategories, setSelectedCategories] = useState<boolean[]>(
    commonStore.categories.map(() => true)
  );

  useEffect(() => {
    const selectedCategoriesIds = selectedCategories
      .map((selected, index) =>
        selected ? commonStore.categories[index].id : -1
      )
      .filter((index) => index !== -1);

    setPostsToShow(
      posts.filter(
        (post) =>
          post.cityId === selectedCity.id &&
          post.categoryIds.every((categoryId) =>
            selectedCategoriesIds.includes(categoryId)
          )
      )
    );
  }, [commonStore, selectedCity, selectedCategories]);

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

  const handlePopupClick = (postId: number) => {
    toast.success(`Post ${postId} clicked!`);
    navigate(Routes.Index);
  };

  return (
    <div className={styles.container}>
      <div className={styles.map}>
        <AddressSearchMap
          searchInputSize={ChakraSizes.Md}
          initialPoint={selectedCity.address.point}
          additionalQuery={selectedCity.name}
          scrollToPoint={selectedCity.address.point}
          isSearchOnly
          elements={postsToShow.map((post) => (
            <Marker
              key={post.id}
              position={[post.address.point.x, post.address.point.y]}
            >
              <Popup>
                <div
                  className={styles.popup}
                  onClick={() => handlePopupClick(post.id)}
                >
                  <p className={styles.title}>{post.title}</p>
                  <p>{post.description}</p>
                </div>
              </Popup>
            </Marker>
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

export default PostMap;
