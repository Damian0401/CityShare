import * as Yup from "yup";

export const searchInputSchema = Yup.object({
  query: Yup.string().notRequired().max(30),
  startDate: Yup.date(),
  endDate: Yup.date()
    .nullable()
    .test({
      name: "endDate",
      message: "endDate must be greater than startDate",
      test: function (value) {
        if (!value || !this.parent.startDate) return true;
        return value > this.parent.startDate;
      },
    })
    .test({
      name: "endDate",
      message: "endDate must be greater than now",
      test: function (value) {
        if (!value) return true;
        return value > new Date();
      },
    }),
  cityId: Yup.number().required(),
  skipCategoryIds: Yup.array().of(Yup.number()).notRequired(),
  sortBy: Yup.string().notRequired(),
});
