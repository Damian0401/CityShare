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
import { observer } from "mobx-react-lite";

const AddressSearchMap: React.FC<IAddressSearchMapProps> = observer((props) => {
  const {
    initialPoint,
    searchInputSize = ChakraSizes.Sm,
    additionalQuery,
    disableSelect = false,
    elements,
    scrollToPoint,
    isSearchVisible = true,
    initialZoom = Constants.Leaflet.Zoom.Default,
    onSelect,
  } = props;

  const [isSelectBlocked, setIsSelectBlocked] =
    useState<boolean>(disableSelect);

  const handleSelect = async (point: IPoint) => {
    if (!onSelect) return;

    const address = await agent.Map.reverse(point);
    onSelect(address);
  };

  const handleMouseEnter = () => {
    if (disableSelect) return;

    setIsSelectBlocked(true);
  };

  const handleMouseLeave = () => {
    if (disableSelect) return;

    setIsSelectBlocked(false);
  };

  return (
    <div className={styles.container}>
      <MapContainer
        center={[initialPoint.x, initialPoint.y]}
        zoom={initialZoom}
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
          {isSearchVisible && (
            <SearchInput
              searchInputSize={searchInputSize}
              additionalQuery={additionalQuery}
            />
          )}
        </div>
      </MapContainer>
    </div>
  );
});

export default AddressSearchMap;
