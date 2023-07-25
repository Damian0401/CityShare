import { nameof } from "ts-simple-nameof";
import * as Yup from "yup";
import { IRegisterValues } from "../../common/interfaces/IRegisterValues";

export const registerSchema = Yup.object({
  email: Yup.string().required().email(),
  userName: Yup.string()
    .required()
    .min(6)
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
    "Passwords do not match"
  ),
});
