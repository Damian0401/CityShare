import { MapContainer, TileLayer, ZoomControl } from "react-leaflet";
import { IAddressSearchMapProps } from "./IAddressSearchMapProps";
import Constants from "../../common/utils/constants";
import styles from "./AddressSearchMap.module.scss";
import { Input, InputGroup, InputLeftElement } from "@chakra-ui/react";
import { SearchIcon } from "@chakra-ui/icons";
import BaseContainer from "../base-container/BaseContainer";
import { Containers, LeafletPositions } from "../../common/enums";
import { ChakraSizes } from "../../common/enums/ChakraSizes";

const AddressSearchMap: React.FC<IAddressSearchMapProps> = ({
  initialPoint,
}) => {
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
      </MapContainer>
      <div className={styles.search}>
        <BaseContainer type={Containers.Primary} className={styles.searchInput}>
          <InputGroup size={ChakraSizes.Sm}>
            <InputLeftElement pointerEvents="none">
              <SearchIcon />
            </InputLeftElement>
            <Input placeholder="Search..." />
          </InputGroup>
        </BaseContainer>
      </div>
    </div>
  );
};

export default AddressSearchMap;
