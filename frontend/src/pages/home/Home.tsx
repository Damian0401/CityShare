import { Containers } from "../../common/enums";
import BaseContainer from "../../components/base-container/BaseContainer";

const Home = () => {
  return (
    <>
      <BaseContainer type={Containers.Primary}>Primary</BaseContainer>
      <br/>
      <BaseContainer type={Containers.Secondary}>Secondary</BaseContainer>
      <br/>
    </>
  );
};

export default Home;
