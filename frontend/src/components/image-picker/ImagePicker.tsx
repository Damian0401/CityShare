import {
  FormControl,
  FormErrorMessage,
  FormLabel,
  Icon,
  Input,
  InputGroup,
  InputLeftElement,
} from "@chakra-ui/react";
import { Cursors, InputTypes } from "../../common/enums";
import { useRef, useState } from "react";
import Constants from "../../common/utils/constants";
import { BsCardImage } from "react-icons/bs";
import { IImagePickerProps } from "./IImagePickerProps";
import SelectedImage from "./components/selected-image/SelectedImage";
import styles from "./ImagePicker.module.scss";

const ImagePicker: React.FC<IImagePickerProps> = (props) => {
  const {
    errors,
    touched,
    label,
    isRequired,
    sizeLimit,
    highlightedImages,
    onImagesChange,
    onImageRemove,
  } = props;

  const inputRef = useRef<HTMLInputElement>(null);

  const [selectedImages, setSelectedImages] = useState<File[]>([]);

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];

    if (!file) {
      return;
    }

    const newImages = [
      ...selectedImages.filter((i) => i.name !== file.name),
      file,
    ];

    setSelectedImages(newImages);
    onImagesChange(newImages);
  };

  const handleImageRemove = (name: string) => {
    const filteredImages = selectedImages.filter(
      (image) => image.name !== name
    );
    setSelectedImages(filteredImages);
    onImagesChange(filteredImages);

    if (onImageRemove) {
      onImageRemove(name);
    }
  };

  return (
    <FormControl isInvalid={!!errors && !!touched} isRequired={isRequired}>
      <FormLabel>{label}</FormLabel>
      <InputGroup>
        <InputLeftElement children={<Icon as={BsCardImage} />} />
        <input
          type={InputTypes.File}
          ref={inputRef}
          style={{ display: Constants.CSS.None }}
          accept={Constants.FileTypes.Image}
          onChange={handleFileChange}
          maxLength={sizeLimit}
        />
        <Input
          onClick={() => inputRef.current?.click()}
          isReadOnly
          cursor={Cursors.Pointer}
        />
      </InputGroup>
      <FormErrorMessage>{errors as string}</FormErrorMessage>
      <div className={styles.imageContainer}>
        {selectedImages.map((image) => (
          <SelectedImage
            key={image.name}
            image={image}
            onRemove={handleImageRemove}
            isHighlighted={highlightedImages?.includes(image.name)}
          />
        ))}
      </div>
    </FormControl>
  );
};

export default ImagePicker;
