import { useMap } from "react-leaflet";
import { IMapControllerProps } from "./IMapControllerProps";
import { useEffect } from "react";
import Constants from "../../../../common/utils/constants";
import { observer } from "mobx-react-lite";

const MapController: React.FC<IMapControllerProps> = observer(
  ({ scrollToPoint }) => {
    const map = useMap();
    useEffect(() => {
      if (!scrollToPoint) return;

      map.setView(
        [scrollToPoint.x, scrollToPoint.y],
        Constants.Leaflet.Zoom.Default
      );
    }, [map, scrollToPoint]);
    return <></>;
  }
);

export default MapController;
