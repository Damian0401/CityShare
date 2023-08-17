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
    cityId: 0,
    address: {
      id: 0,
      displayName: "",
      boundingBox: {
        maxX: 0,
        maxY: 0,
        minX: 0,
        minY: 0,
      },
      point: {
        x: 0,
        y: 0,
      },
    },
    categoryIds: [],
    images: [],
  },
};
