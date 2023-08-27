import { format } from "date-fns";
import { Containers, Routes } from "../../../../../common/enums";
import BaseContainer from "../../../../../components/base-container/BaseContainer";
import { ISearchResultProps } from "./ISearchResultProps";
import styles from "./SearchResult.module.scss";
import { observer } from "mobx-react-lite";
import { useStore } from "../../../../../common/stores/store";
import { IPost } from "../../../../../common/interfaces";
import {
  AiFillMinusSquare,
  AiFillPlusSquare,
  AiOutlineMinusSquare,
  AiOutlinePlusSquare,
} from "react-icons/ai";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import { MouseEvent } from "react";

const SearchResult: React.FC<ISearchResultProps> = observer((props) => {
  const { posts } = props;

  const { commonStore } = useStore();

  const navigate = useNavigate();

  const getPostStatus = (post: IPost) => {
    const currentDate = new Date();
    if (post.endDate < currentDate) {
      return "Finished";
    }

    if (post.startDate < currentDate) {
      return "Started";
    }

    return "Not started";
  };

  const handlePostClick = (postId: number) => {
    navigate(Routes.Posts + "/" + postId);
  };

  const handleScoreClick = (
    postId: number,
    isLiked: boolean,
    event: MouseEvent<HTMLSpanElement>
  ) => {
    event.stopPropagation();

    if (isLiked) {
      toast.success(`Post ${postId} score increased`);
      return;
    }

    toast.error(`Post ${postId} score decreased`);
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
            onClick={() => handlePostClick(post.id)}
          >
            <div className={styles.image}>
              <img
                src={
                  post.imageUrls.length > 0
                    ? post.imageUrls[0]
                    : "https://picsum.photos/200/300"
                }
                alt="post"
              />
            </div>
            <div className={styles.header}>
              <div>
                <span className={styles.title}>{post.title}</span>{" "}
                {post.categoryIds.map((id) => (
                  <span key={id} className={styles.category}>
                    {commonStore.categories.find((x) => x.id === id)?.name}
                  </span>
                ))}
              </div>
              <div className={styles.score}>
                <span
                  className={styles.icon}
                  onClick={(e) => handleScoreClick(post.id, true, e)}
                >
                  {post.isLiked ? (
                    <AiFillPlusSquare />
                  ) : (
                    <AiOutlinePlusSquare />
                  )}
                </span>
                <span>{post.score}</span>
                <span
                  className={styles.icon}
                  onClick={(e) => handleScoreClick(post.id, false, e)}
                >
                  {post?.isLiked === false ? (
                    <AiFillMinusSquare />
                  ) : (
                    <AiOutlineMinusSquare />
                  )}
                </span>
              </div>
            </div>
            <div className={styles.body}>
              <div>{getPostStatus(post)}</div>
              <div>{post.description}</div>
            </div>
            <div className={styles.footer}>
              <div>
                <div>
                  {format(new Date(post.createdAt), "dd/MM/yyyy HH:mm")}
                </div>
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
