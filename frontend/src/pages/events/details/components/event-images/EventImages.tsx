import { useEffect, useState } from "react";
import styles from "./EventImages.module.scss";
import { IEventImagesProps } from "./IEventImagesProps";
import { observer } from "mobx-react-lite";

const EventImages: React.FC<IEventImagesProps> = observer(({ imageUrls }) => {
  const [selectedImageIndex, setSelectedImageIndex] = useState<number>(0);
  useEffect(() => {
    if (imageUrls.length === 0) return;

    const changeImageIntervalId = setInterval(() => {
      setSelectedImageIndex((prev) =>
        prev === imageUrls.length - 1 ? 0 : prev + 1
      );
    }, 10000);

    return () => clearInterval(changeImageIntervalId);
  }, [imageUrls.length, setSelectedImageIndex]);

  return (
    <div className={styles.container}>
      <img
        className={styles.main}
        src={
          imageUrls.length > 0
            ? imageUrls[selectedImageIndex]
            : "https://picsum.photos/200/300"
        }
        alt="event"
      />
      {imageUrls.length > 1 && (
        <div className={styles.preview}>
          {imageUrls.map((url, index) => (
            <img
              key={index}
              src={url}
              alt="event"
              onClick={() => setSelectedImageIndex(index)}
            />
          ))}
        </div>
      )}
    </div>
  );
});

export default EventImages;
