import { Button } from "@chakra-ui/react";
import { InputTypes, Routes } from "../../common/enums";
import styles from "./Login.module.scss";
import { ILoginValues } from "../../common/interfaces";
import { nameof } from "ts-simple-nameof";
import { Link, useNavigate } from "react-router-dom";
import { Formik } from "formik";
import TextInput from "../../components/text-input/TextInput";
import { loginSchema } from "./LoginSchema";
import { useStore } from "../../common/stores/store";
import { observer } from "mobx-react-lite";
import { useState } from "react";
import { InitialValues } from "../../common/utils/initialValues";

const Login = observer(() => {
  const [isLoading, setIsLoading] = useState(false);
  const { authStore } = useStore();
  const navigate = useNavigate();

  const handleSubmit = async (values: ILoginValues) => {
    setIsLoading(true);
    try {
      await authStore.login(values);
      navigate(Routes.Index);
    } catch {
      setIsLoading(false);
    }
  };

  return (
    <div className={styles.container}>
      <Formik
        initialValues={InitialValues.Login}
        onSubmit={handleSubmit}
        validationSchema={loginSchema}
      >
        {({ handleSubmit, errors, touched }) => (
          <form onSubmit={handleSubmit} className={styles.formContainer}>
            <TextInput
              label="Email"
              type={InputTypes.Email}
              errors={errors.email}
              touched={touched.email}
              isRequired
              name={nameof<ILoginValues>((x) => x.email)}
            />
            <TextInput
              label="Password"
              type={InputTypes.Password}
              name={nameof<ILoginValues>((x) => x.password)}
              errors={errors.password}
              touched={touched.password}
              isRequired
            />
            <div className={styles.buttonContainer}>
              <Link to={Routes.Register} className={styles.link}>
                No account? Register
              </Link>
              <Button type="submit" isLoading={isLoading}>
                Login
              </Button>
            </div>
          </form>
        )}
      </Formik>
    </div>
  );
});

export default Login;
