import * as Yup from "yup";
import { getSecret } from "../../../common/utils/helpers";
import { Environments } from "../../../common/enums";

export const postCreateSchema = Yup.object({
  title: Yup.string().required(),
  description: Yup.string().required().max(1000),
  cityId: Yup.number().required().positive("city is a required field"),
  address: Yup.object({
    displayName: Yup.string().required("address is a required field"),
  }).required(),
  categoryIds: Yup.array()
    .required()
    .max(getSecret(Environments.MaxCategoryNumber))
    .min(getSecret(Environments.MinCategoryNumber)),
  images: Yup.array()
    .max(getSecret(Environments.MaxImageNumber))
    .min(getSecret(Environments.MinImageNumber)),
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
