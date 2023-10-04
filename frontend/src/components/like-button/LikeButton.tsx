import { ILikeButtonProps } from "./ILikeButtonProps";
import { AiFillLike, AiOutlineLike } from "react-icons/ai";
import { MouseEvent } from "react";
import styles from "./LikeButton.module.scss";

const LikeButton: React.FC<ILikeButtonProps> = (props) => {
  const { id, likes, isLiked, className, onLike } = props;
  const handleLikeClick = (event: MouseEvent<HTMLSpanElement>) => {
    event.stopPropagation();
    onLike(id);
  };
  return (
    <div className={`${styles.container} ${className}`}>
      <span className={styles.icon} onClick={(e) => handleLikeClick(e)}>
        {isLiked ? <AiFillLike /> : <AiOutlineLike />}
      </span>
      <span>{likes}</span>
    </div>
  );
};

export default LikeButton;
