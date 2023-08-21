export interface IImagePickerProps {
  errors?: string;
  touched?: boolean;
  name: string;
  label: string;
  isRequired?: boolean;
  onImagesChange: (images: File[]) => void;
}
