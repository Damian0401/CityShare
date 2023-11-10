import { Containers } from "../../common/enums";
import { IProfile } from "../../common/interfaces/IProfile";
import BaseContainer from "../../components/base-container/BaseContainer";
import styles from "./Profile.module.scss";
import IncreasingNumber from "./components/increasing-number/IncreasingNumber";

const data: IProfile = {
  userName: "John Doe",
  email: "johnDoe@email.com",
  createdEvents: 5,
  givenLikes: 10,
  receivedLikes: 15,
  givenComments: 10,
  receivedComments: 15,
};

const Profile = () => {
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
        Receoved likes: <IncreasingNumber number={data.receivedLikes} />
      </div>
      <div>
        Given comments: <IncreasingNumber number={data.givenComments} />
      </div>
      <div>
        Received comments: <IncreasingNumber number={data.receivedComments} />
      </div>
    </BaseContainer>
  );
};

export default Profile;
