import { observer } from "mobx-react-lite";
import styles from "./Requsts.module.scss";
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
import { ChangeEvent, useCallback, useEffect, useState } from "react";
import { toast } from "react-toastify";
import CollapsableRequest from "./components/collapsable-request/CollapsableRequest";
import { IRequests } from "../../common/interfaces";

const Requsts = observer(() => {
  const { commonStore, requestStore } = useStore();

  const [requests, setRequests] = useState<IRequests>();

  const getCityId = useCallback(
    () => getSelectedCityId() ?? commonStore.cities[0].id,
    [commonStore.cities]
  );

  useEffect(() => {
    commonStore.setLoading(true);

    const abortController = new AbortController();

    const loadRequests = async () => {
      const requests = await requestStore.getRequests(
        getCityId(),
        abortController.signal
      );
      setRequests(requests);
      commonStore.setLoading(false);
    };

    loadRequests();

    return () => abortController.abort();
  }, [commonStore, requestStore, getCityId]);

  const handleSelect = async (event: ChangeEvent<HTMLSelectElement>) => {
    localStorage.setItem(StorageKeys.SelectedCityId, event.target.value);
    commonStore.setLoading(true);
    const requests = await requestStore.getRequests(
      parseInt(event.target.value)
    );
    setRequests(requests);
    commonStore.setLoading(false);
  };

  const handleOnAccept = async (id: string) => {
    await requestStore.acceptRequest(id);
    toast.success("Accepted");
    await refrestRequests(id);
  };

  const handleOnReject = async (id: string) => {
    await requestStore.rejectRequest(id);
    toast.error("Rejected");
    await refrestRequests(id);
  };

  const refrestRequests = async (requestId: string) => {
    requestStore.removeRequest(getCityId(), requestId);
    const newRequests = await requestStore.getRequests(getCityId());
    setRequests(newRequests);
  };

  return (
    <div className={styles.container}>
      <div>Requests:</div>
      <Select defaultValue={getCityId()} onChange={handleSelect}>
        {commonStore.cities.map((city) => (
          <option key={city.id} value={city.id}>
            {city.name}
          </option>
        ))}
      </Select>
      <Tabs isFitted>
        <TabList className={styles.tab}>
          {commonStore.requestTypes.map((requestType) => (
            <Tab key={requestType.id}>{requestType.name}</Tab>
          ))}
        </TabList>
        <TabPanels>
          {commonStore.requestTypes.map((requestType) => (
            <TabPanel key={requestType.id} className={styles.requests}>
              {requests?.requests.get(requestType.id)?.map((blurRequest) => (
                <CollapsableRequest
                  key={blurRequest.id}
                  request={blurRequest}
                  onAccept={handleOnAccept}
                  onReject={handleOnReject}
                />
              ))}
            </TabPanel>
          ))}
        </TabPanels>
      </Tabs>
    </div>
  );
});

export default Requsts;
