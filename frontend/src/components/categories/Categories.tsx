import { observer } from "mobx-react-lite";
import styles from "./Categories.module.scss";
import { ICategoryProps } from "./ICategoriesProps";
import { useStore } from "../../common/stores/store";

const Categories: React.FC<ICategoryProps> = observer(({ categoryIds }) => {
  const { commonStore } = useStore();
  return categoryIds.map((categoryId) => (
    <span className={styles.container} key={categoryId}>
      {commonStore.categories.find((x) => x.id === categoryId)?.name}
    </span>
  ));
});

export default Categories;
