import { IUser } from "../../../../common/interfaces/IUser";

export interface IUserMenuProps {
  user: IUser;
  logout: () => void;
}
