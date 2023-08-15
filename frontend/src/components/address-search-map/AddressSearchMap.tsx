import { MapContainer, TileLayer, ZoomControl } from "react-leaflet";
import { IAddressSearchMapProps } from "./IAddressSearchMapProps";
import Constants from "../../common/utils/constants";
import styles from "./AddressSearchMap.module.scss";
import { LeafletPositions } from "../../common/enums";
import { ChakraSizes } from "../../common/enums";
import SearchInput from "./components/search-input/SearchInput";
import SelectMarker from "./components/select-marker/SelectMarker";
import { IPoint } from "../../common/interfaces";
import agent from "../../common/api/agent";
import { useState } from "react";
import MapController from "./components/map-controller/MapController";

const AddressSearchMap: React.FC<IAddressSearchMapProps> = (props) => {
  const {
    initialPoint,
    searchInputSize = ChakraSizes.Sm,
    additionalQuery,
    isSearchOnly = false,
    elements,
    scrollToPoint,
    onSelect,
  } = props;

  const [isSelectBlocked, setIsSelectBlocked] = useState<boolean>(isSearchOnly);

  const handleSelect = async (point: IPoint) => {
    if (!onSelect) return;

    const address = await agent.Map.reverse(point);
    onSelect(address);
  };

  const handleMouseEnter = () => {
    if (isSearchOnly) return;

    setIsSelectBlocked(true);
  };

  const handleMouseLeave = () => {
    if (isSearchOnly) return;

    setIsSelectBlocked(false);
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
        <MapController scrollToPoint={scrollToPoint} />
        {elements && elements.map((marker) => marker)}
        <div
          className={styles.search}
          onMouseEnter={handleMouseEnter}
          onMouseLeave={handleMouseLeave}
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
