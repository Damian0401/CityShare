import { useEffect } from "react";
import LoadingSpinner from "../../components/loading-spinner/LoadingSpinner";
import { useNavigate, useSearchParams } from "react-router-dom";
import { toast } from "react-toastify";
import { Routes } from "../../common/enums";
import { useStore } from "../../common/stores/store";
import Constants from "../../common/utils/constants";

const ConfirmEmail = () => {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();

  const { authStore } = useStore();

  useEffect(() => {
    const id = searchParams.get("id");
    const token = searchParams.get("token");

    if (!id || !token) {
      navigate(Routes.NotFound);
      return;
    }

    const controller = new AbortController();
    const confirmEmail = async () => {
      try {
        await authStore.confirmEmail(id, token, controller.signal);
        toast.success("Email confirmed");
        navigate(Routes.Index);
      } catch {
        setTimeout(() => {
          navigate(Routes.Index);
        }, Constants.RedirectTimeout);
      }
    };

    try {
      confirmEmail();
    } catch (_) {
      navigate(Routes.Index);
    }

    return () => controller.abort();
  }, [searchParams, authStore, navigate]);
  return <LoadingSpinner />;
};

export default ConfirmEmail;
