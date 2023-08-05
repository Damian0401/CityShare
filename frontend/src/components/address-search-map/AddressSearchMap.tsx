import { MapContainer, TileLayer, ZoomControl } from "react-leaflet";
import { IAddressSearchMapProps } from "./IAddressSearchMapProps";
import Constants from "../../common/utils/constants";
import styles from "./AddressSearchMap.module.scss";
import { LeafletPositions } from "../../common/enums";
import { ChakraSizes } from "../../common/enums/ChakraSizes";
import SearchInput from "./components/search-input/SearchInput";
import SelectMarker from "./components/select-marker/SelectMarker";
import { IPoint } from "../../common/interfaces";
import agent from "../../common/api/agent";
import { useState } from "react";

const AddressSearchMap: React.FC<IAddressSearchMapProps> = (props) => {
  const {
    initialPoint,
    searchInputSize = ChakraSizes.Sm,
    additionalQuery,
    onSelect,
  } = props;

  const [isSelectBlocked, setIsSelectBlocked] = useState<boolean>(false);

  const handleSelect = async (point: IPoint) => {
    const address = await agent.Map.reverse(point);
    onSelect(address);
  };

  return (
    <div className={styles.container}>
      <MapContainer
        center={[initialPoint.x, initialPoint.y]}
        zoom={Constants.Leaflet.Zoom.Default}
        zoomControl={false}
      >
        <TileLayer
          attribution={Constants.Leaflet.Attribution}
          url={Constants.Leaflet.Url}
        />
        <SelectMarker
          onSelect={handleSelect}
          isSelectBlocked={isSelectBlocked}
        />
        <ZoomControl position={LeafletPositions.BottomLeft} />
        <div
          className={styles.search}
          onMouseEnter={() => setIsSelectBlocked(true)}
          onMouseLeave={() => setIsSelectBlocked(false)}
        >
          <SearchInput
            searchInputSize={searchInputSize}
            additionalQuery={additionalQuery}
          />
        </div>
      </MapContainer>
    </div>
  );
};

export default AddressSearchMap;
