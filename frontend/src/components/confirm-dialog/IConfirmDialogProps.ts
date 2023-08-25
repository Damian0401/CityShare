export interface IConfirmDialogProps {
  isOpen: boolean;
  header?: string;
  body?: string;
  onClose: () => void;
  onConfirm: () => void;
}
