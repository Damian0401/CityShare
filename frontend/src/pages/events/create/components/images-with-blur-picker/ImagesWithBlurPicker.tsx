import { useDisclosure } from "@chakra-ui/react";
import { IImagesWithBlurPickerProps } from "./IImagesWithBlurPickerProps";
import { useState } from "react";
import {
  INewImage
} from "../../../../../common/interfaces";
import ImagePicker from "../../../../../components/image-picker/ImagePicker";
import ConfirmDialog from "../../../../../components/confirm-dialog/ConfirmDialog";

const ImagesWithBlurPicker: React.FC<IImagesWithBlurPickerProps> = (props) => {
  const { setImages, setImagesTouched, sizeLimit, allImages, errors, touched } = props;

  const {
    isOpen: isBlurDialogOpen,
    onOpen: onBlurDialogOpen,
    onClose: onBlurDialogClose,
  } = useDisclosure();

  const [imagesToBlur, setImagesToBlur] = useState<string[]>([]);

  const [selectedImage, setSelectedImage] = useState<string>("");

  const handleOpenBlurDialog = (image: string) => {
    setSelectedImage(image);
    onBlurDialogOpen();
  };

  const handleImagesChange = async (images: File[], isNewImage: boolean) => {
    const mappedImages: INewImage[] = images.map((image) => ({
      image: image,
      shouldBeBlurred: imagesToBlur.includes(image.name),
    }));

    await setImages(mappedImages);
    await setImagesTouched(true);

    if (images.length === 0 || !isNewImage) {
      return;
    }

    handleOpenBlurDialog(images[images.length - 1].name);
  };

  const handleBlurDialogConfirm = async () => {
    onBlurDialogClose();
    const newImagesToBlur = [...imagesToBlur, selectedImage];

    const mappedImages: INewImage[] = allImages.map((image) => ({
      image: image.image,
      shouldBeBlurred: newImagesToBlur.includes(image.image.name),
    }));

    setImagesToBlur(newImagesToBlur);
    setImages(mappedImages);
  };

  return (
    <>
      <ImagePicker
        label="Images"
        errors={errors}
        touched={touched}
        sizeLimit={sizeLimit}
        isRequired
        highlightedImages={imagesToBlur}
        onImagesChange={async (newImages) =>
          handleImagesChange(newImages, newImages.length > allImages.length)
        }
        onImageRemove={(name) => {
          setImagesToBlur(imagesToBlur.filter((image) => image !== name));
        }}
      />
      <ConfirmDialog
        isOpen={isBlurDialogOpen}
        onClose={onBlurDialogClose}
        onConfirm={handleBlurDialogConfirm}
        header="Do you want to blur faces from this image?"
      />
    </>
  );
};

export default ImagesWithBlurPicker;
