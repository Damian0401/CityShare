import { observer } from "mobx-react-lite";
import styles from "./Requsts.module.scss";
import { IRequest as IRequest } from "../../common/interfaces/IRequest";
import {
  Select,
  Tab,
  TabList,
  TabPanel,
  TabPanels,
  Tabs,
} from "@chakra-ui/react";
import { useStore } from "../../common/stores/store";
import { getSelectedCityId } from "../../common/utils/helpers";
import { StorageKeys } from "../../common/enums";
import { ChangeEvent } from "react";
import { toast } from "react-toastify";
import CollapsableRequest from "./components/collapsable-request/CollapsableRequest";

const requests: IRequest[] = [
  {
    id: "1",
    message: "Description 1",
    author: "Author 1",
    createdAt: new Date(),
    eventId: "face78f3-ac84-43d8-650a-08dbd26f5602",
  },
  {
    id: "2",
    message: "Description 2",
    author: "Author 2",
    createdAt: new Date(),
    eventId: "face78f3-ac84-43d8-650a-08dbd26f5602",
  },
  {
    id: "3",
    message: "Description 3",
    author: "Author 3",
    createdAt: new Date(),
    eventId: "face78f3-ac84-43d8-650a-08dbd26f5602",
  },
  {
    id: "4",
    message: "Description 1",
    author: "Author 1",
    createdAt: new Date(),
    eventId: "face78f3-ac84-43d8-650a-08dbd26f5602",
  },
  {
    id: "5",
    message: "Description 2",
    author: "Author 2",
    createdAt: new Date(),
    eventId: "face78f3-ac84-43d8-650a-08dbd26f5602",
  },
  {
    id: "6",
    message: "Description 3",
    author: "Author 3",
    createdAt: new Date(),
    eventId: "face78f3-ac84-43d8-650a-08dbd26f5602",
  },
  {
    id: "7",
    message: "Description 1",
    author: "Author 1",
    createdAt: new Date(),
    eventId: "face78f3-ac84-43d8-650a-08dbd26f5602",
  },
  {
    id: "8",
    message: "Description 2",
    author: "Author 2",
    createdAt: new Date(),
    eventId: "face78f3-ac84-43d8-650a-08dbd26f5602",
  },
  {
    id: "9",
    message: "Description 3",
    author: "Author 3",
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
      <div>Requests:</div>
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
      <Tabs isFitted>
        <TabList className={styles.tab}>
          <Tab>Blur</Tab>
          <Tab>Delete</Tab>
        </TabList>
        <TabPanels>
          <TabPanel className={styles.requests}>
            {requests.map((blurRequest) => (
              <CollapsableRequest
                key={blurRequest.id}
                request={blurRequest}
                onAccept={() => toast.success("Accepted")}
                onReject={() => toast.error("Rejected")}
              />
            ))}
          </TabPanel>
          <TabPanel className={styles.requests}>
            {requests
              .filter((x) => x.message.endsWith("2"))
              .map((blurRequest) => (
                <CollapsableRequest
                  key={blurRequest.id}
                  request={blurRequest}
                  onAccept={() => toast.success("Accepted")}
                  onReject={() => toast.error("Rejected")}
                />
              ))}
          </TabPanel>
        </TabPanels>
      </Tabs>
    </div>
  );
});

export default BlurRequsts;
