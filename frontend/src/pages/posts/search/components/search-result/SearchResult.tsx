import { ISearchResultProps } from "./ISearchResultProps";
import styles from "./SearchResult.module.scss";

const SearchResult: React.FC<ISearchResultProps> = (props) => {
  const { posts } = props;
  return (
    <div className={styles.container}>
      {posts.length === 0 ? (
        <p>No posts found</p>
      ) : (
        posts.map((post) => (
          <div key={post.id}>
            <p>{post.title}</p>
            <p>{post.description}</p>
          </div>
        ))
      )}
    </div>
  );
};

export default SearchResult;
