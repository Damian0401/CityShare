export interface IImagePickerProps {
  errors?: string;
  touched?: boolean;
  label: string;
  sizeLimit?: number;
  isRequired?: boolean;
  highlightedImages?: string[];
  onImageRemove?: (name: string) => void;
  onImagesChange: (images: File[]) => void;
}
