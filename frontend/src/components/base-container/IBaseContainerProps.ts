import { Containers } from "../../common/enums";

export interface IBaseContainerProps {
    children: React.ReactNode;
    type: Containers;
}