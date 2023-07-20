import { Button } from "@chakra-ui/react";
import { Containers, InputTypes } from "../../common/enums";
import BaseContainer from "../../components/base-container/BaseContainer";
import styles from "./Register.module.scss";
import PasswordInput from "../../components/password-input/PasswordInput";
import { nameof } from "ts-simple-nameof";
import { Link } from "react-router-dom";
import { Formik } from "formik";
import * as Yup from "yup";
import TextInput from "../../components/text-input/TextInput";
import { IRegisterValues } from "../../common/interfaces/IRegisterValues";

const Register = () => {
  const initialValues: IRegisterValues = {
    email: "",
    userName: "",
    password: "",
    confirmPassword: "",
  };

  const validationSchema = Yup.object({
    email: Yup.string().required().email(),
    userName: Yup.string()
      .required()
      .matches(
        /^[a-zA-Z0-9]+$/,
        "Username must contain only letters and numbers"
      ),
    password: Yup.string()
      .required()
      .min(6)
      .matches(/[A-Z]/, "Password must contain at least one uppercase letter")
      .matches(/[a-z]/, "Password must contain at least one lowercase letter")
      .matches(/[0-9]/, "Password must contain at least one number")
      .matches(/[^a-zA-Z0-9]/, "Password must contain at least one symbol"),
    confirmPassword: Yup.string().oneOf(
      [Yup.ref(nameof<IRegisterValues>((x) => x.password))],
      "Passwords must match"
    ),
  });

  const handleSubmit = (values: IRegisterValues) => {
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
            <PasswordInput
              label="Password"
              name={nameof<IRegisterValues>((x) => x.password)}
              errors={errors.password}
              touched={touched.password}
              isRequired
            />
            <PasswordInput
              label="Confirm Password"
              name={nameof<IRegisterValues>((x) => x.confirmPassword)}
              errors={errors.confirmPassword}
              touched={touched.confirmPassword}
              isRequired
            />
            <div className={styles.buttonContainer}>
              <Link to="/login" className={styles.link}>
                Already have an account?
              </Link>
              <Button type="submit">Register</Button>
            </div>
          </form>
        )}
      </Formik>
    </BaseContainer>
  );
};

export default Register;
