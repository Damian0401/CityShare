import { observer } from "mobx-react-lite";
import styles from "./Categories.module.scss";
import { ICategoryProps } from "./ICategoriesProps";
import { useStore } from "../../common/stores/store";

const Category: React.FC<ICategoryProps> = observer(({ categoryIds }) => {
  const { commonStore } = useStore();
  return categoryIds.map((categoryId) => (
    <span className={styles.container}>
      {commonStore.categories.find((x) => x.id === categoryId)?.name}
    </span>
  ));
});

export default Category;
