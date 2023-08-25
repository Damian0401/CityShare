import { Button, Divider, useDisclosure } from "@chakra-ui/react";
import styles from "./PostCreate.module.scss";
import { Formik } from "formik";
import { InitialValues } from "../../../common/utils/initialValues";
import { postCreateSchema } from "./PostCreateSchema";
import TextInput from "../../../components/text-input/TextInput";
import { ButtonTypes, InputTypes, Routes } from "../../../common/enums";
import { nameof } from "ts-simple-nameof";
import { IPostCreateValues } from "../../../common/interfaces";
import { observer } from "mobx-react-lite";
import { useStore } from "../../../common/stores/store";
import { useNavigate } from "react-router-dom";
import { useEffect } from "react";
import OptionSelect from "../../../components/option-select/OptionSelect";
import { toast } from "react-toastify";
import AddressPickerModal from "./components/address-picker-modal/AddressPickerModal";
import MultiOptionSelect from "../../../components/multi-option-select/MultiOptionSelect";
import DateTimePicker from "../../../components/date-time-picker/DateTimePicker";
import ImagesWithBlurPicker from "./components/images-with-blur-picker/ImagesWithBlurPicker";

const PostCreate = observer(() => {
  const {
    authStore: { user },
    commonStore,
  } = useStore();

  const navigate = useNavigate();

  const {
    isOpen: isAddressModalOpen,
    onOpen: onAddressModalOpen,
    onClose: onAddressModalClose,
  } = useDisclosure();

  useEffect(() => {
    if (!user?.emailConfirmed) {
      toast.info("Please confirm your email address first");
      navigate(Routes.Profile);
    }
  }, [user, navigate]);

  const handleAddressClick = (cityId: number | string) => {
    if (cityId === -1 || cityId === "-1") {
      toast.warning("Please select a city first");
      return;
    }

    onAddressModalOpen();
  };

  return (
    <div className={styles.container}>
      <div className={styles.title}>Create a new event to share</div>
      <Divider />
      <Formik
        initialValues={InitialValues.PostCreate}
        onSubmit={console.log}
        validationSchema={postCreateSchema}
      >
        {({
          handleSubmit,
          setFieldValue,
          setFieldTouched,
          values,
          errors,
          touched,
        }) => (
          <form onSubmit={handleSubmit} className={styles.formContainer}>
            <TextInput
              label="Title"
              type={InputTypes.Text}
              errors={errors.title}
              touched={touched.title}
              isRequired
              name={nameof<IPostCreateValues>((x) => x.title)}
            />
            <TextInput
              label="Description"
              type={InputTypes.Text}
              errors={errors.description}
              touched={touched.description}
              isRequired
              isMultiline
              name={nameof<IPostCreateValues>((x) => x.description)}
            />
            <OptionSelect
              label="City"
              options={[
                { value: -1, label: "" },
                ...commonStore.cities.map((city) => ({
                  label: city.name,
                  value: city.id,
                })),
              ]}
              name={nameof<IPostCreateValues>((x) => x.cityId)}
              errors={errors.cityId}
              touched={touched.cityId}
              isRequired
            />
            <TextInput
              label="Address"
              type={InputTypes.Text}
              errors={errors.address?.displayName}
              touched={touched.address?.displayName}
              isRequired
              isReadOnly
              name={nameof<IPostCreateValues>((x) => x.address.displayName)}
              onClick={() => handleAddressClick(values.cityId)}
            />
            <DateTimePicker
              label="Start Date"
              name={nameof<IPostCreateValues>((x) => x.startDate)}
              errors={errors.startDate as string}
              touched={touched.startDate as boolean}
              isRequired
            />
            <DateTimePicker
              label="End Date"
              name={nameof<IPostCreateValues>((x) => x.endDate)}
              errors={errors.endDate as string}
              touched={touched.endDate as boolean}
              isRequired
            />
            <MultiOptionSelect
              errors={errors.categoryIds}
              touched={touched.categoryIds}
              name={nameof<IPostCreateValues>((x) => x.categoryIds)}
              label="Categories"
              isRequired
              options={commonStore.categories.map((category) => ({
                label: category.name,
                value: category.id,
              }))}
              onChange={async (values) => {
                await setFieldValue(
                  nameof<IPostCreateValues>((x) => x.categoryIds),
                  values
                );

                await setFieldTouched(
                  nameof<IPostCreateValues>((x) => x.categoryIds),
                  true
                );
              }}
            />
            <ImagesWithBlurPicker
              errors={errors.images as string}
              touched={touched.images as unknown as boolean}
              allImages={values.images}
              setImages={async (images) => {
                await setFieldValue(
                  nameof<IPostCreateValues>((x) => x.images),
                  images
                );
              }}
              setImagesTouched={async (value) => {
                await setFieldTouched(
                  nameof<IPostCreateValues>((x) => x.images),
                  value
                );
              }}
            />
            <div className={styles.buttonContainer}>
              <Button type={ButtonTypes.Submit}>Create</Button>
            </div>
            <AddressPickerModal
              isOpen={isAddressModalOpen}
              onClose={onAddressModalClose}
              onSelect={(address) => {
                setFieldValue(
                  nameof<IPostCreateValues>((x) => x.address),
                  address
                );
                onAddressModalClose();
              }}
              cityId={values.cityId}
            />
          </form>
        )}
      </Formik>
    </div>
  );
});

export default PostCreate;
