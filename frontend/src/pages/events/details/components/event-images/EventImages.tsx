import { useEffect, useState } from "react";
import styles from "./EventImages.module.scss";
import { IEventImagesProps } from "./IEventImagesProps";
import { observer } from "mobx-react-lite";
import Constants from "../../../../../common/utils/constants";

const EventImages: React.FC<IEventImagesProps> = observer(({ imageUrls }) => {
  const [selectedImageIndex, setSelectedImageIndex] = useState<number>(0);
  useEffect(() => {
    if (imageUrls.length === 0) return;

    const changeImageIntervalId = setInterval(() => {
      setSelectedImageIndex((prev) =>
        prev === imageUrls.length - 1 ? 0 : prev + 1
      );
    }, Constants.ChangeImageInterval);

    return () => clearInterval(changeImageIntervalId);
  }, [imageUrls.length, setSelectedImageIndex]);

  return (
    <div className={styles.container}>
      <img
        className={imageUrls.length > 1 ? styles.mainSmall : styles.mainBig}
        src={
          imageUrls.length > 0
            ? imageUrls[selectedImageIndex] ?? Constants.Images.Urls.Processing
            : Constants.Images.Urls.Placeholder
        }
        alt={Constants.Images.Alts.Event}
      />
      {imageUrls.length > 1 && (
        <div className={styles.preview}>
          {imageUrls.map((url, index) => (
            <img
              key={index}
              src={url ?? Constants.Images.Urls.Processing}
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
