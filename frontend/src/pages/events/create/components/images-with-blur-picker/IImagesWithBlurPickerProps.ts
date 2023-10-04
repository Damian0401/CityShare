import { IEventImage } from "../../../../../common/interfaces";

export interface IImagesWithBlurPickerProps {
  setImages: (values: IEventImage[]) => Promise<void>;
  setImagesTouched: (value: boolean) => Promise<void>;
  errors: string;
  touched: boolean;
  allImages: IEventImage[];
  sizeLimit?: number;
}
