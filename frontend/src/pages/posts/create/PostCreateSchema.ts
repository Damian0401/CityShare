import * as Yup from "yup";

export const postCreateSchema = Yup.object({
  title: Yup.string().required(),
  description: Yup.string().required().max(1000),
  cityId: Yup.number().required().positive("city is a required field"),
  address: Yup.object({
    displayName: Yup.string().required("address is a required field"),
  }).required(),
  categoryIds: Yup.array().required().max(3).min(1),
  images: Yup.array().max(5).min(0),
  startDate: Yup.date().required(),
  endDate: Yup.date()
    .required()
    .test({
      name: "endDate",
      message: "endDate must be greater than startDate",
      test: function (value) {
        return value > this.parent.startDate;
      },
    })
    .test({
      name: "endDate",
      message: "endDate must be greater than now",
      test: function (value) {
        return value > new Date();
      },
    }),
});
