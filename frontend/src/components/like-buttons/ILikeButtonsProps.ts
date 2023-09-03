export interface ILikeButtonsProps {
  id: number;
  likes: number;
  isLiked?: boolean;
  className?: string;
  onLike: (id: number, isLiked: boolean) => void;
}
