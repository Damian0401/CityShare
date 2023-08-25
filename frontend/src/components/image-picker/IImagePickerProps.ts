export interface IImagePickerProps {
  errors?: string;
  touched?: boolean;
  name: string;
  label: string;
  isRequired?: boolean;
  highlightedImages?: string[];
  onImageRemove?: (name: string) => void;
  onImagesChange: (images: File[]) => void;
}
