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
  const handleLikeClick = (
    event: MouseEvent<HTMLSpanElement>,
    isLiked: boolean
  ) => {
    event.stopPropagation();
    onLike(id, isLiked);
  };
  return (
    <div className={`${styles.container} ${className}`}>
      <span className={styles.icon} onClick={(e) => handleLikeClick(e, true)}>
        {isLiked ? <AiFillLike /> : <AiOutlineLike />}
      </span>
      <span>{likes}</span>
      <span className={styles.icon} onClick={(e) => handleLikeClick(e, false)}>
        {isLiked === false ? <AiFillDislike /> : <AiOutlineDislike />}
      </span>
    </div>
  );
};

export default LikeButtons;
