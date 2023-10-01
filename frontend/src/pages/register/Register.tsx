import { Button } from "@chakra-ui/react";
import { ButtonTypes, InputTypes, Routes } from "../../common/enums";
import styles from "./Register.module.scss";
import { nameof } from "ts-simple-nameof";
import { Link, useNavigate } from "react-router-dom";
import { Formik } from "formik";
import TextInput from "../../components/text-input/TextInput";
import { registerSchema } from "./RegisterSchema";
import { useStore } from "../../common/stores/store";
import { observer } from "mobx-react-lite";
import { useState } from "react";
import { IRegisterValues } from "../../common/interfaces";
import { InitialValues } from "../../common/utils/initialValues";

const Register = observer(() => {
  const [isLoading, setIsLoading] = useState(false);
  const { authStore, commonStore } = useStore();
  const navigate = useNavigate();

  const handleSubmit = async (values: IRegisterValues) => {
    setIsLoading(true);
    try {
      await authStore.register(values);
      await commonStore.loadCommonData();
      navigate(Routes.Index);
    } catch {
      setIsLoading(false);
    }
  };

  return (
    <div className={styles.container}>
      <Formik
        initialValues={InitialValues.Register}
        onSubmit={handleSubmit}
        validationSchema={registerSchema}
      >
        {({ handleSubmit, errors, touched }) => (
          <form onSubmit={handleSubmit} className={styles.formContainer}>
            <TextInput
              label="Email"
              type={InputTypes.Email}
              errors={errors.email}
              touched={touched.email}
              isRequired
              name={nameof<IRegisterValues>((x) => x.email)}
            />
            <TextInput
              label="User Name"
              type={InputTypes.Text}
              errors={errors.userName}
              touched={touched.userName}
              isRequired
              name={nameof<IRegisterValues>((x) => x.userName)}
            />
            <TextInput
              label="Password"
              type={InputTypes.Password}
              name={nameof<IRegisterValues>((x) => x.password)}
              errors={errors.password}
              touched={touched.password}
              isRequired
            />
            <TextInput
              label="Confirm Password"
              type={InputTypes.Password}
              name={nameof<IRegisterValues>((x) => x.confirmPassword)}
              errors={errors.confirmPassword}
              touched={touched.confirmPassword}
              isRequired
            />
            <div className={styles.buttonContainer}>
              <Link to={Routes.Login} className={styles.link}>
                Already have an account?
              </Link>
              <Button type={ButtonTypes.Submit} isLoading={isLoading}>
                Register
              </Button>
            </div>
          </form>
        )}
      </Formik>
    </div>
  );
});

export default Register;
