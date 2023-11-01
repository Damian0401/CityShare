import { useEffect, useState } from "react";
import styles from "./EventImages.module.scss";
import { IEventImagesProps } from "./IEventImagesProps";
import { observer } from "mobx-react-lite";
import Constants from "../../../../../common/utils/constants";

const EventImages: React.FC<IEventImagesProps> = observer(({ images }) => {
  const [selectedImageIndex, setSelectedImageIndex] = useState<number>(0);
  useEffect(() => {
    if (images.length === 0) return;

    const changeImageIntervalId = setInterval(() => {
      setSelectedImageIndex((prev) =>
        prev === images.length - 1 ? 0 : prev + 1
      );
    }, Constants.ChangeImageInterval);

    return () => clearInterval(changeImageIntervalId);
  }, [images.length, setSelectedImageIndex]);

  return (
    <div className={styles.container}>
      <img
        className={images.length > 1 ? styles.mainSmall : styles.mainBig}
        src={
          images.length > 0
            ? images[selectedImageIndex].uri ?? Constants.Images.Urls.Processing
            : Constants.Images.Urls.Placeholder
        }
        alt={Constants.Images.Alts.Event}
      />
      {images.length > 1 && (
        <div className={styles.preview}>
          {images.map((image, index) => (
            <img
              key={image.id}
              src={image.uri ?? Constants.Images.Urls.Processing}
              alt={Constants.Images.Alts.Event}
              onClick={() => setSelectedImageIndex(index)}
            />
          ))}
        </div>
      )}
    </div>
  );
});

export default EventImages;
