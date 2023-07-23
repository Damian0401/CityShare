import { IPageWrapperProps } from "./IPageWrapperProps";
import { Outlet } from "react-router-dom";
import styles from "./PageWrapper.module.scss";
import { observer } from "mobx-react-lite";

const PageWrapper = observer(({ Element }: IPageWrapperProps) => {
  return (
    <>
      <div className={styles.container}>
        {Element && <Element />}
        <Outlet />
      </div>
    </>
  );
});

export default PageWrapper;
