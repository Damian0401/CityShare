export interface IRequestsModalProps {
  isOpen: boolean;
  imageUri: string;
  imageId: string;
  imageAlt?: string;
  onClose: () => void;
}
