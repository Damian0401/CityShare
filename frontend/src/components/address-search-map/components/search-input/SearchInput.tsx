import { Input, InputGroup, InputLeftElement } from "@chakra-ui/react";
import { Containers, Cursors } from "../../../../common/enums";
import BaseContainer from "../../../base-container/BaseContainer";
import { ISearchInputProps } from "./ISearchInputProps";
import styles from "./SearchInput.module.scss";
import { SearchIcon } from "@chakra-ui/icons";
import { useRef } from "react";
import agent from "../../../../common/api/agent";
import { useMap } from "react-leaflet";
import Constants from "../../../../common/utils/constants";

const SearchInput: React.FC<ISearchInputProps> = (props) => {
  const { searchInputSize } = props;

  const map = useMap();

  const searchRef = useRef<HTMLInputElement>(null);

  const handleSearch = async () => {
    if (!searchRef.current) return;

    const searchResult = await agent.Map.search(searchRef.current.value);

    map.setView(
      [searchResult.x, searchResult.y],
      Constants.Leaflet.Zoom.Search
    );
  };

  return (
    <BaseContainer type={Containers.Primary} className={styles.container}>
      <InputGroup size={searchInputSize}>
        <InputLeftElement onClick={handleSearch} cursor={Cursors.Pointer}>
          <SearchIcon />
        </InputLeftElement>
        <Input ref={searchRef} placeholder="Search..." />
      </InputGroup>
    </BaseContainer>
  );
};

export default SearchInput;
