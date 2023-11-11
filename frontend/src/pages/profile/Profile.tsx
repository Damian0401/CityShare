import { useEffect, useState } from "react";
import { Containers } from "../../common/enums";
import { IProfile } from "../../common/interfaces/IProfile";
import BaseContainer from "../../components/base-container/BaseContainer";
import styles from "./Profile.module.scss";
import IncreasingNumber from "../../components/increasing-number/IncreasingNumber";
import LoadingSpinner from "../../components/loading-spinner/LoadingSpinner";
import { observer } from "mobx-react-lite";
import { useStore } from "../../common/stores/store";

const Profile = observer(() => {
  const [data, setData] = useState<IProfile>();

  const { authStore } = useStore();

  useEffect(() => {
    const loadData = async () => {
      const data = await authStore.getProfile();
      setData(data);
    };

    loadData();
  }, [authStore]);

  if (!data) return <LoadingSpinner />;

  return (
    <BaseContainer type={Containers.Primary} className={styles.container}>
      <div>
        Name: <span>{data.userName}</span>
      </div>
      <div>
        Email: <span>{data.email}</span>
      </div>
      <div>
        Given likes: <IncreasingNumber number={data.givenLikes} />
      </div>
      <div>
        Received likes: <IncreasingNumber number={data.receivedLikes} />
      </div>
      <div>
        Given comments: <IncreasingNumber number={data.givenComments} />
      </div>
      <div>
        Received comments: <IncreasingNumber number={data.receivedComments} />
      </div>
    </BaseContainer>
  );
});

export default Profile;
