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
  const { searchInputSize, additionalQuery } = props;

  const map = useMap();

  const searchRef = useRef<HTMLInputElement>(null);

  const handleSearch = async () => {
    if (!searchRef.current) return;

    let query = searchRef.current.value;

    if (additionalQuery) query += `, ${additionalQuery}`;

    const searchResult = await agent.Map.search(query);

    map.setView(
      [searchResult.point.x, searchResult.point.y],
      Constants.Leaflet.Zoom.Search
    );
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    console.log(e.key);
    if (e.key !== "Enter") return;
    handleSearch();
  };

  return (
    <BaseContainer type={Containers.Primary} className={styles.container}>
      <InputGroup size={searchInputSize}>
        <InputLeftElement onClick={handleSearch} cursor={Cursors.Pointer}>
          <SearchIcon />
        </InputLeftElement>
        <Input
          ref={searchRef}
          onKeyDown={handleKeyDown}
          placeholder="Search..."
        />
      </InputGroup>
    </BaseContainer>
  );
};

export default SearchInput;
