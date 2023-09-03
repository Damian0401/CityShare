import { observer } from "mobx-react-lite";
import { IEventMapProps } from "./IEventMapProps";
import styles from "./EventMap.module.scss";
import AddressSearchMap from "../../../../../components/address-search-map/AddressSearchMap";
import { Marker, Popup } from "react-leaflet";
import Constants from "../../../../../common/utils/constants";

const EventMap: React.FC<IEventMapProps> = observer(({ address }) => {
  return (
    <div className={styles.container}>
      <AddressSearchMap
        initialPoint={address.point}
        disableSelect
        isSearchVisible={false}
        elements={[
          <Marker position={[address.point.x, address.point.y]} key="marker">
            <Popup>
              <div>{address.displayName}</div>
            </Popup>
          </Marker>,
        ]}
        initialZoom={Constants.Leaflet.Zoom.Search}
      />
    </div>
  );
});

export default EventMap;
