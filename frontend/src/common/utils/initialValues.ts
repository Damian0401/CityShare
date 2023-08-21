import { IInitialValues } from "../interfaces";

export const InitialValues: IInitialValues = {
  Login: {
    email: "",
    password: "",
  },
  Register: {
    email: "",
    password: "",
    confirmPassword: "",
    userName: "",
  },
  PostCreate: {
    title: "",
    description: "",
    cityId: -1,
    address: {
      displayName: "",
      point: {
        x: 0,
        y: 0,
      },
    },
    categoryIds: [],
    images: [],
    startDate: new Date(),
    endDate: new Date(),
  },
};
