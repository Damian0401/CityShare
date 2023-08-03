import { Marker, useMapEvents } from "react-leaflet";
import { ISelectMarkerProps } from "./ISelectMarkerProps";
import { useState } from "react";
import { IPoint } from "../../../../common/interfaces";

const SelectMarker: React.FC<ISelectMarkerProps> = (props) => {
  const { onSelect, isSelectBlocked } = props;

  const [position, setPosition] = useState<IPoint>();

  useMapEvents({
    click: (e) => {
      if (isSelectBlocked) return;
      setPosition({ x: e.latlng.lat, y: e.latlng.lng });
      onSelect({ x: e.latlng.lat, y: e.latlng.lng });
    },
  });

  return <>{position && <Marker position={[position.x, position.y]} />}</>;
};

export default SelectMarker;
