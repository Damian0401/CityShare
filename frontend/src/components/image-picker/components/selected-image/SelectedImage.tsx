import { Tooltip } from "@chakra-ui/react";
import { ISelectedImageProps } from "./ISelectedImageProps";
import styles from "./SelectedImage.module.scss";
import Constants from "../../../../common/utils/constants";

const SelectedImage: React.FC<ISelectedImageProps> = (props) => {
  const { image, onRemove, isHighlighted } = props;

  return (
    <div className={styles.container} onClick={() => onRemove(image.name)}>
      <Tooltip
        label="Click to remove"
        aria-label={Constants.AriaLabels.RemoveImage}
      >
        <img
          src={URL.createObjectURL(image)}
          alt={image.name}
          className={styles.image}
          style={{ opacity: isHighlighted ? 0.5 : 1 }}
        />
      </Tooltip>
    </div>
  );
};

export default SelectedImage;
