import { useEffect } from "react";
import LoadingSpinner from "../../components/loading-spinner/LoadingSpinner";
import { useSearchParams } from "react-router-dom";
import { toast } from "react-toastify";
import Router from "../Router";
import { Routes } from "../../common/enums";
import { useStore } from "../../common/stores/store";

const ConfirmEmail = () => {
  const [searchParams] = useSearchParams();

  const { authStore } = useStore();

  useEffect(() => {
    const id = searchParams.get("id");
    const token = searchParams.get("token");

    if (!id || !token) {
      Router.navigate(Routes.NotFound);
      return;
    }

    const controller = new AbortController();
    const confirmEmail = async () => {
      await authStore.confirmEmail(id, token, controller.signal);
      toast.success("Email confirmed");
      Router.navigate(Routes.Index);
    };

    try {
      confirmEmail();
    } catch (_) {
      Router.navigate(Routes.Index);
    }

    return () => controller.abort();
  }, [searchParams, authStore]);
  return <LoadingSpinner />;
};

export default ConfirmEmail;
