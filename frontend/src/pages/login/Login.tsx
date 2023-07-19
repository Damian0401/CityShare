import { Button } from "@chakra-ui/react";
import { Containers, InputTypes } from "../../common/enums";
import BaseContainer from "../../components/base-container/BaseContainer";
import styles from "./Login.module.scss";
import PasswordInput from "../../components/password-input/PasswordInput";
import { ILoginValues } from "../../common/interfaces";
import { nameof } from "ts-simple-nameof";
import { Link } from "react-router-dom";
import { Formik } from "formik";
import * as Yup from "yup";
import TextInput from "../../components/text-input/TextInput";

const Login = () => {
  const initialValues: ILoginValues = {
    email: "",
    password: "",
  };

  const validationSchema = Yup.object({
    email: Yup.string().required().email(),
    password: Yup.string().required(),
  });

  const handleSubmit = (values: ILoginValues) => {
    console.log(values);
  };

  return (
    <BaseContainer type={Containers.Primary} className={styles.container}>
      <Formik
        initialValues={initialValues}
        onSubmit={handleSubmit}
        validationSchema={validationSchema}
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
            <PasswordInput
              label="Password"
              name={nameof<ILoginValues>((x) => x.password)}
              errors={errors.password}
              touched={touched.password}
              isRequired
            />
            <div className={styles.buttonContainer}>
              <Link to="/register" className={styles.link}>
                No account? Register
              </Link>
              <Button type="submit">Login</Button>
            </div>
          </form>
        )}
      </Formik>
    </BaseContainer>
  );
};

export default Login;
