import { MapContainer, TileLayer, ZoomControl } from "react-leaflet";
import { IAddressSearchMapProps } from "./IAddressSearchMapProps";
import Constants from "../../common/utils/constants";
import styles from "./AddressSearchMap.module.scss";
import { LeafletPositions } from "../../common/enums";
import { ChakraSizes } from "../../common/enums/ChakraSizes";
import SearchInput from "./components/search-input/SearchInput";

const AddressSearchMap: React.FC<IAddressSearchMapProps> = (props) => {
  const { initialPoint, searchInputSize = ChakraSizes.Sm } = props;

  return (
    <div className={styles.container}>
      <MapContainer
        center={[initialPoint.x, initialPoint.y]}
        zoom={13}
        zoomControl={false}
      >
        <TileLayer
          attribution={Constants.LeafletAttribution}
          url={Constants.LeafletUrl}
        />
        <ZoomControl position={LeafletPositions.BottomLeft} />
        <div className={styles.search}>
          <SearchInput searchInputSize={searchInputSize} />
        </div>
      </MapContainer>
    </div>
  );
};

export default AddressSearchMap;
