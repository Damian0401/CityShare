import { INewImage } from "../../../../../common/interfaces";

export interface IImagesWithBlurPickerProps {
  setImages: (values: INewImage[]) => Promise<void>;
  setImagesTouched: (value: boolean) => Promise<void>;
  errors: string;
  touched: boolean;
  allImages: INewImage[];
}
