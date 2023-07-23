import * as Yup from "yup";

export const loginSchema = Yup.object({
  email: Yup.string().required().email(),
  password: Yup.string().required(),
});
