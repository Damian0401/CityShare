import { IPageWrapperProps } from "./IPageWrapperProps";
import { Outlet, useNavigate } from "react-router-dom";
import styles from "./PageWrapper.module.scss";
import { observer } from "mobx-react-lite";
import { useStore } from "../../common/stores/store";
import { accessTokenHelper } from "../../common/utils/helpers";
import { useEffect, useState } from "react";
import { Containers, Routes } from "../../common/enums";
import LoadingSpinner from "../loading-spinner/LoadingSpinner";
import BaseContainer from "../base-container/BaseContainer";

const PageWrapper = observer(({ Element }: IPageWrapperProps) => {
  const [isLoading, setIsLoading] = useState(true);
  const { authStore, commonStore } = useStore();
  const navigate = useNavigate();

  useEffect(() => {
    const isTokenStored = accessTokenHelper.isAccessTokenPresent();

    if (!isTokenStored) {
      navigate(Routes.Login);
      setIsLoading(false);
      return;
    }

    const refreshUser = async () => {
      try {
        await authStore.refresh();
        await commonStore.loadCommonData();
      } catch {
        await authStore.logout();
        Routes.Login;
      } finally {
        setIsLoading(false);
      }
    };

    refreshUser();
  }, [authStore, commonStore, navigate]);

  return (
    <>
      <div className={styles.container}>
        {isLoading ? (
          <LoadingSpinner />
        ) : (
          <>
            {Element && <Element />}
            <BaseContainer type={Containers.Primary}>
              <Outlet />
            </BaseContainer>
          </>
        )}
      </div>
    </>
  );
});

export default PageWrapper;
