export interface ILikeButtonProps {
  id: string;
  likes: number;
  isLiked?: boolean;
  className?: string;
  onLike: (id: string, isLiked: boolean) => void;
}
