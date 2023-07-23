import { Button } from "@chakra-ui/react";
import { Containers, InputTypes } from "../../common/enums";
import BaseContainer from "../../components/base-container/BaseContainer";
import styles from "./Register.module.scss";
import PasswordInput from "../../components/password-input/PasswordInput";
import { nameof } from "ts-simple-nameof";
import { Link, useNavigate } from "react-router-dom";
import { Formik } from "formik";
import TextInput from "../../components/text-input/TextInput";
import { IRegisterValues } from "../../common/interfaces/IRegisterValues";
import { registerSchema } from "./RegisterSchema";
import { useStore } from "../../common/stores/store";
import { observer } from "mobx-react-lite";

const Register = observer(() => {
  const { authStore } = useStore();

  const navigate = useNavigate();

  const initialValues: IRegisterValues = {
    email: "",
    userName: "",
    password: "",
    confirmPassword: "",
  };

  const handleSubmit = async (values: IRegisterValues) => {
    await authStore.register(values);
    navigate("/");
  };

  return (
    <BaseContainer type={Containers.Primary} className={styles.container}>
      <Formik
        initialValues={initialValues}
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
});

export default Register;
