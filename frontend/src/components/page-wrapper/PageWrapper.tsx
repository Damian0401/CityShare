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
        navigate(Routes.Login);
      } finally {
        setIsLoading(false);
      }
    };

    refreshUser();
  }, [authStore, commonStore, navigate]);

  return (
    <>
      <div className={styles.wrapper}>
        {isLoading ? (
          <LoadingSpinner />
        ) : (
          <>
            {Element && <Element />}
            <main>
              <BaseContainer
                type={Containers.Primary}
                className={styles.container}
              >
                <Outlet />
              </BaseContainer>
            </main>
          </>
        )}
      </div>
    </>
  );
});

export default PageWrapper;
