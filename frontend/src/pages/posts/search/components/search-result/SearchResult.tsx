import { format } from "date-fns";
import { Containers } from "../../../../../common/enums";
import BaseContainer from "../../../../../components/base-container/BaseContainer";
import { ISearchResultProps } from "./ISearchResultProps";
import styles from "./SearchResult.module.scss";
import { observer } from "mobx-react-lite";
import { useStore } from "../../../../../common/stores/store";

const SearchResult: React.FC<ISearchResultProps> = observer((props) => {
  const { posts } = props;

  const { commonStore } = useStore();

  const isStarted = (startDate: Date) => {
    return startDate.getTime() < new Date().getTime();
  };

  return (
    <div className={styles.container}>
      {posts.length === 0 ? (
        <p>No posts found</p>
      ) : (
        posts.map((post) => (
          <BaseContainer
            type={Containers.Tertiary}
            className={styles.post}
            key={post.id}
          >
            <div className={styles.image}>
              <img src={post.imageUrls[0]} alt="post" />
            </div>
            <div className={styles.header}>
              <div>
                {post.title}{" "}
                {post.categoryIds.map((id) => (
                  <span key={id} className={styles.category}>
                    {commonStore.categories.find((x) => x.id === id)?.name}
                  </span>
                ))}
              </div>
              <div>{format(new Date(post.createdAt), "dd/MM/yyyy HH:mm")}</div>
            </div>
            <div className={styles.body}>
              <div>
                Status:{" "}
                {isStarted(new Date(post.startDate))
                  ? "Started"
                  : "Not started"}
              </div>
              <div>{post.description}</div>
            </div>
            <div className={styles.footer}>
              <div>
                {!isStarted(new Date(post.startDate)) ? (
                  <span>
                    Start:{" "}
                    {format(new Date(post.startDate), "dd/MM/yyyy HH:mm")}
                  </span>
                ) : (
                  <span>
                    End: {format(new Date(post.endDate), "dd/MM/yyyy HH:mm")}
                  </span>
                )}
              </div>
              <div>{post.author}</div>
            </div>
          </BaseContainer>
        ))
      )}
    </div>
  );
});

export default SearchResult;
