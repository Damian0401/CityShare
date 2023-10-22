import { observer } from "mobx-react-lite";
import styles from "./BlurRequsts.module.scss";
import { IBlurRequest } from "../../common/interfaces/IBlurRequest";
import { Select } from "@chakra-ui/react";
import { useStore } from "../../common/stores/store";
import { getSelectedCityId } from "../../common/utils/helpers";
import { StorageKeys } from "../../common/enums";
import { ChangeEvent } from "react";
import { toast } from "react-toastify";
import CollapsableRequest from "./components/collapsable-request/CollapsableRequest";

const blurRequests: IBlurRequest[] = [
  {
    id: "1",
    title: "Blur request 1",
    description: "Description 1",
    author: "Author 1",
    cityId: 1,
    createdAt: new Date(),
    eventId: "face78f3-ac84-43d8-650a-08dbd26f5602",
  },
  {
    id: "2",
    title: "Blur request 2",
    description: "Description 2",
    author: "Author 2",
    cityId: 2,
    createdAt: new Date(),
    eventId: "face78f3-ac84-43d8-650a-08dbd26f5602",
  },
  {
    id: "3",
    title: "Blur request 3",
    description: "Description 3",
    author: "Author 3",
    cityId: 3,
    createdAt: new Date(),
    eventId: "face78f3-ac84-43d8-650a-08dbd26f5602",
  },
  {
    id: "4",
    title: "Blur request 1",
    description: "Description 1",
    author: "Author 1",
    cityId: 1,
    createdAt: new Date(),
    eventId: "face78f3-ac84-43d8-650a-08dbd26f5602",
  },
  {
    id: "5",
    title: "Blur request 2",
    description: "Description 2",
    author: "Author 2",
    cityId: 2,
    createdAt: new Date(),
    eventId: "face78f3-ac84-43d8-650a-08dbd26f5602",
  },
  {
    id: "6",
    title: "Blur request 3",
    description: "Description 3",
    author: "Author 3",
    cityId: 3,
    createdAt: new Date(),
    eventId: "face78f3-ac84-43d8-650a-08dbd26f5602",
  },
  {
    id: "7",
    title: "Blur request 1",
    description: "Description 1",
    author: "Author 1",
    cityId: 1,
    createdAt: new Date(),
    eventId: "face78f3-ac84-43d8-650a-08dbd26f5602",
  },
  {
    id: "8",
    title: "Blur request 2",
    description: "Description 2",
    author: "Author 2",
    cityId: 2,
    createdAt: new Date(),
    eventId: "face78f3-ac84-43d8-650a-08dbd26f5602",
  },
  {
    id: "9",
    title: "Blur request 3",
    description: "Description 3",
    author: "Author 3",
    cityId: 3,
    createdAt: new Date(),
    eventId: "face78f3-ac84-43d8-650a-08dbd26f5602",
  },
];

const BlurRequsts = observer(() => {
  const { commonStore } = useStore();

  const handleSelect = (event: ChangeEvent<HTMLSelectElement>) => {
    console.log(event.target.value);
    localStorage.setItem(StorageKeys.SelectedCityId, event.target.value);
  };

  return (
    <div className={styles.container}>
      <div>Blur requests:</div>
      <Select
        defaultValue={getSelectedCityId() ?? commonStore.cities[0].id}
        onChange={handleSelect}
      >
        {commonStore.cities.map((city) => (
          <option key={city.id} value={city.id}>
            {city.name}
          </option>
        ))}
      </Select>
      <div className={styles.requests}>
        {blurRequests.map((blurRequest) => (
          <CollapsableRequest
            key={blurRequest.id}
            request={blurRequest}
            onAccept={() => toast.success("Accepted")}
            onReject={() => toast.error("Rejected")}
          />
        ))}
      </div>
    </div>
  );
});

export default BlurRequsts;
