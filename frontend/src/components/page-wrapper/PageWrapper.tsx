import { IPageWrapperProps } from "./IPageWrapperProps";
import { Outlet } from "react-router-dom";
import styles from "./PageWrapper.module.scss";
import { observer } from "mobx-react-lite";
import { useStore } from "../../common/stores/store";
import { accessTokenHelper } from "../../common/utils/helpers";
import { useEffect, useState } from "react";
import Router from "../../pages/Router";
import { Routes } from "../../common/enums";
import LoadingSpinner from "../loading-spinner/LoadingSpinner";

const PageWrapper = observer(({ Element }: IPageWrapperProps) => {
  const [isLoading, setIsLoading] = useState(true);
  const { authStore } = useStore();

  useEffect(() => {
    const isTokenStored = accessTokenHelper.isAccessTokenPresent();

    if (!isTokenStored) {
      Router.navigate(Routes.Login);
      setIsLoading(false);
      return;
    }

    const refreshUser = async () => {
      try {
        await authStore.refresh();
      } catch {
        await authStore.logout();
        Router.navigate(Routes.Login);
      } finally {
        setIsLoading(false);
      }
    };

    refreshUser();
  }, [authStore]);

  return (
    <>
      <div className={styles.container}>
        {isLoading ? (
          <LoadingSpinner />
        ) : (
          <>
            {Element && <Element />}
            <Outlet />
          </>
        )}
      </div>
    </>
  );
});

export default PageWrapper;
