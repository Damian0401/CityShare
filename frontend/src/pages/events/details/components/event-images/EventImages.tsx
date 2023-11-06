import { useEffect, useState } from "react";
import styles from "./EventImages.module.scss";
import { IEventImagesProps } from "./IEventImagesProps";
import { observer } from "mobx-react-lite";
import Constants from "../../../../../common/utils/constants";
import { useDisclosure } from "@chakra-ui/react";
import RequestsModal from "./components/requests-modal/RequestsModal";
import { useSearchParams } from "react-router-dom";
import { SearchParams } from "../../../../../common/enums";
import { toast } from "react-toastify";

const EventImages: React.FC<IEventImagesProps> = observer(({ images }) => {
  const [selectedImageIndex, setSelectedImageIndex] = useState<number>(0);
  const [openImageIndex, setOpenImageIndex] = useState<number>(0);
  const [searchParams] = useSearchParams();
  const {
    isOpen: isModalOpen,
    onOpen: onModalOpen,
    onClose: onModalClose,
  } = useDisclosure();

  useEffect(() => {
    const imageId = searchParams.get(SearchParams.ImageId);

    if (!imageId) return;

    const imageIndex = images.findIndex((image) => image.id === imageId);

    if (imageIndex === -1) {
      toast.error("Image not found");
      return;
    }

    setOpenImageIndex(imageIndex);
    onModalOpen();
  }, [searchParams, images, setOpenImageIndex, onModalOpen]);

  useEffect(() => {
    if (images.length === 0) return;

    const changeImageIntervalId = setInterval(() => {
      setSelectedImageIndex((prev) =>
        prev === images.length - 1 ? 0 : prev + 1
      );
    }, Constants.ChangeImageInterval);

    return () => clearInterval(changeImageIntervalId);
  }, [images.length, setSelectedImageIndex]);

  const handleImageClick = () => {
    setOpenImageIndex(selectedImageIndex);
    onModalOpen();
  };

  return (
    <div className={styles.container}>
      <img
        onClick={images.length > 0 ? handleImageClick : undefined}
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
      <RequestsModal
        isOpen={isModalOpen}
        onClose={onModalClose}
        imageUri={
          images[openImageIndex].uri ?? Constants.Images.Urls.Processing
        }
        imageId={images[openImageIndex].id}
      />
    </div>
  );
});

export default EventImages;
