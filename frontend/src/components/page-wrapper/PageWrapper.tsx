import { IPageWrapperProps } from "./IPageWrapperProps";
import { Outlet, useNavigate } from "react-router-dom";
import styles from "./PageWrapper.module.scss";
import { observer } from "mobx-react-lite";
import { useStore } from "../../common/stores/store";
import { AccessTokenHelper } from "../../common/utils/helpers";
import { useEffect, useState } from "react";
import { AxiosErrorCodes, Containers, Routes } from "../../common/enums";
import LoadingSpinner from "../loading-spinner/LoadingSpinner";
import BaseContainer from "../base-container/BaseContainer";
import { AxiosError } from "axios";

const PageWrapper = observer(({ Element }: IPageWrapperProps) => {
  const [isPageLoading, setIsPageLoading] = useState(true);
  const { authStore, commonStore } = useStore();
  const navigate = useNavigate();

  useEffect(() => {
    const isTokenStored = AccessTokenHelper.isAccessTokenPresent();

    if (!isTokenStored) {
      navigate(Routes.Login);
      setIsPageLoading(false);
      return;
    }

    const controller = new AbortController();
    const refreshUser = async () => {
      try {
        await authStore.refresh(controller.signal);
        await commonStore.loadCommonData();
      } catch (error) {
        if (
          error instanceof AxiosError &&
          error.code === AxiosErrorCodes.Canceled
        ) {
          return;
        }

        await authStore.logout();
        navigate(Routes.Login);
      }
      setIsPageLoading(false);
    };

    refreshUser();

    return () => controller.abort();
  }, [authStore, commonStore, navigate]);

  return (
    <>
      <div className={styles.wrapper}>
        {isPageLoading ? (
          <LoadingSpinner />
        ) : (
          <>
            {Element && <Element />}
            <main>
              {commonStore.isContentLoading && <LoadingSpinner />}
              <div
                style={{
                  visibility: commonStore.isContentLoading
                    ? "hidden"
                    : undefined,
                }}
              >
                <BaseContainer
                  type={Containers.Primary}
                  className={styles.container}
                >
                  <Outlet />
                </BaseContainer>
              </div>
            </main>
          </>
        )}
      </div>
    </>
  );
});

export default PageWrapper;
