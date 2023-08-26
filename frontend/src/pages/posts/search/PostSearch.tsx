import styles from "./PostSearch.module.scss";
import { IPost } from "../../../common/interfaces";
import SearchInput from "./components/search-input/SearchInput";
import SearchResult from "./components/search-result/SearchResult";
import { IPostSearchQuery } from "../../../common/interfaces/IPostSearchQuery";
import { observer } from "mobx-react-lite";

const posts: IPost[] = [
  {
    id: 1,
    title: "Title 1",
    description: "Description 1",
    createdAt: new Date(),
    startDate: new Date(),
    endDate: new Date(),
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
    score: 5,
    author: "Author 1",
  },
  {
    id: 2,
    title: "Title 2",
    description: "Description 2",
    createdAt: new Date(),
    startDate: new Date(),
    endDate: new Date(),
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
    score: 5,
    author: "Author 2",
  },
  {
    id: 3,
    title: "Title 3",
    description: "Description 3",
    createdAt: new Date(),
    startDate: new Date(),
    endDate: new Date(),
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
    score: 5,
    author: "Author 3",
  },
];

const PostSearch = observer(() => {
  const handleSearch = (query: IPostSearchQuery) => {
    console.log(query);
  };
  return (
    <div className={styles.container}>
      <SearchInput onSearch={handleSearch} />
      <SearchResult posts={[]} />
    </div>
  );
});

export default PostSearch;
