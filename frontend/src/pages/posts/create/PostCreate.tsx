import { Divider } from "@chakra-ui/react";
import styles from "./PostCreate.module.scss";

const PostCreate = () => {
  return (
    <div className={styles.container}>
      <div className={styles.title}>Create a new event to share</div>
      <Divider />
    </div>
  );
};

export default PostCreate;
