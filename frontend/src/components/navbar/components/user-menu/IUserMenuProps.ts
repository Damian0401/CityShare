import { IUser } from "../../../../common/interfaces";

export interface IUserMenuProps {
  user: IUser;
  logout: () => void;
}
