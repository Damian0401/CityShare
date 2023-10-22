import { observer } from "mobx-react-lite";
import styled from "./AdminPanel.module.scss";

const AdminPanel = observer(() => {
  return <div className={styled.container}>Admin panel</div>;
});

export default AdminPanel;
