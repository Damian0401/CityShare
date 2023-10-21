import {
  Modal,
  ModalBody,
  ModalCloseButton,
  ModalContent,
  ModalHeader,
  ModalOverlay,
} from "@chakra-ui/react";
import { IAddressPickerModalProps } from "./IAddressPickerModalProps";
import { useStore } from "../../../../../common/stores/store";
import { useEffect, useState } from "react";
import { IAddress, ICity } from "../../../../../common/interfaces";
import AddressSearchMap from "../../../../../components/address-search-map/AddressSearchMap";
import styles from "./AddressPickerModal.module.scss";
import { observer } from "mobx-react-lite";
import { isPointInsideBoundingBox } from "../../../../../common/utils/helpers";
import { toast } from "react-toastify";

const AddressPickerModal: React.FC<IAddressPickerModalProps> = observer(
  (props) => {
    const { isOpen, onClose, cityId, onSelect } = props;

    const [city, setCity] = useState<ICity>();

    const { commonStore } = useStore();

    useEffect(() => {
      const city = commonStore.cities.find(
        (city) => city.id === parseInt(cityId as string)
      );

      if (!city) return;

      setCity(city);
    }, [cityId, commonStore.cities]);

    const handleSelect = (address: IAddress) => {
      if (
        !city ||
        !isPointInsideBoundingBox(address.point, city.address.boundingBox)
      ) {
        toast.error("Address is outside of the city");
        return;
      }

      onSelect(address);
    };

    return (
      <>
        <Modal isOpen={isOpen} onClose={onClose} isCentered>
          <ModalOverlay />
          <ModalContent className={styles.container}>
            <ModalHeader className={styles.header}>
              Pick a location on the map
            </ModalHeader>
            <ModalCloseButton />
            <ModalBody>
              {isOpen && city && (
                <div className={styles.mapContainer}>
                  <AddressSearchMap
                    initialPoint={city.address.point}
                    additionalQuery={city.name}
                    onSelect={handleSelect}
                  />
                </div>
              )}
            </ModalBody>
          </ModalContent>
        </Modal>
      </>
    );
  }
);

export default AddressPickerModal;
