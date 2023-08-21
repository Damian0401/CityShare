import { Tooltip } from "@chakra-ui/react";
import { ISelectedImageProps } from "./ISelectedImageProps";
import styles from "./SelectedImage.module.scss";
import Constants from "../../../../common/utils/constants";

const SelectedImage: React.FC<ISelectedImageProps> = (props) => {
  const { image, onRemove } = props;

  return (
    <div className={styles.container} onClick={() => onRemove(image.name)}>
      <Tooltip
        label="Remove image"
        aria-label={Constants.AriaLabels.RemoveImage}
      >
        <img
          src={URL.createObjectURL(image)}
          alt={image.name}
          className={styles.image}
        />
      </Tooltip>
    </div>
  );
};

export default SelectedImage;
