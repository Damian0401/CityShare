import { ILikeButtonsProps } from "./ILikeButtonsProps";
import {
  AiFillDislike,
  AiFillLike,
  AiOutlineDislike,
  AiOutlineLike,
} from "react-icons/ai";
import { MouseEvent } from "react";
import styles from "./LikeButtons.module.scss";

const LikeButtons: React.FC<ILikeButtonsProps> = (props) => {
  const { id, likes, isLiked, className, onLike } = props;
  const handleLikeClick = (event: MouseEvent<HTMLSpanElement>) => {
    event.stopPropagation();
    onLike(id, true);
  };
  return (
    <div className={`${styles.container} ${className}`}>
      <span className={styles.icon} onClick={handleLikeClick}>
        {isLiked ? <AiFillLike /> : <AiOutlineLike />}
      </span>
      <span>{likes}</span>
      <span className={styles.icon} onClick={handleLikeClick}>
        {isLiked === false ? <AiFillDislike /> : <AiOutlineDislike />}
      </span>
    </div>
  );
};

export default LikeButtons;
