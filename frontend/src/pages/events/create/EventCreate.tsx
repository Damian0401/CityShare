import { Button, Divider, useDisclosure } from "@chakra-ui/react";
import styles from "./EventCreate.module.scss";
import { Formik } from "formik";
import { InitialValues } from "../../../common/utils/initialValues";
import { EventCreateSchema } from "./EventCreateSchema";
import TextInput from "../../../components/text-input/TextInput";
import { ButtonTypes, InputTypes, Routes } from "../../../common/enums";
import { nameof } from "ts-simple-nameof";
import { IEventCreateValues } from "../../../common/interfaces";
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
import Constants from "../../../common/utils/constants";

const EventCreate = observer(() => {
  const {
    authStore: { user },
    commonStore,
    eventStore,
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

  const handleSubmit = async (values: IEventCreateValues) => {
    const id = await eventStore.createEvent(values);

    toast.success("Event created successfully");

    navigate(`${Routes.Events}/${id}`);
  };

  return (
    <div className={styles.container}>
      <div className={styles.title}>Create a new event to share</div>
      <Divider />
      <Formik
        initialValues={InitialValues.EventCreate}
        onSubmit={handleSubmit}
        validationSchema={EventCreateSchema}
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
              name={nameof<IEventCreateValues>((x) => x.title)}
            />
            <TextInput
              label="Description"
              type={InputTypes.Text}
              errors={errors.description}
              touched={touched.description}
              isRequired
              isMultiline
              name={nameof<IEventCreateValues>((x) => x.description)}
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
              name={nameof<IEventCreateValues>((x) => x.cityId)}
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
              name={nameof<IEventCreateValues>((x) => x.address.displayName)}
              onClick={() => handleAddressClick(values.cityId)}
            />
            <DateTimePicker
              label="Start Date"
              errors={errors.startDate as string}
              touched={touched.startDate as boolean}
              isRequired
              onChange={(date) => {
                setFieldValue(
                  nameof<IEventCreateValues>((x) => x.startDate),
                  date
                );
              }}
            />
            <DateTimePicker
              label="End Date"
              errors={errors.endDate as string}
              touched={touched.endDate as boolean}
              isRequired
              onChange={(date) =>
                setFieldValue(
                  nameof<IEventCreateValues>((x) => x.endDate),
                  date
                )
              }
            />
            <MultiOptionSelect
              errors={errors.categoryIds}
              touched={touched.categoryIds}
              name={nameof<IEventCreateValues>((x) => x.categoryIds)}
              label="Categories"
              isRequired
              options={commonStore.categories.map((category) => ({
                label: category.name,
                value: category.id,
              }))}
              onChange={async (values) => {
                await setFieldValue(
                  nameof<IEventCreateValues>((x) => x.categoryIds),
                  values
                );

                await setFieldTouched(
                  nameof<IEventCreateValues>((x) => x.categoryIds),
                  true
                );
              }}
            />
            <ImagesWithBlurPicker
              errors={errors.images as string}
              touched={touched.images as unknown as boolean}
              allImages={values.images ?? []}
              sizeLimit={5 * Constants.FileSizes.MB}
              setImages={async (images) => {
                await setFieldValue(
                  nameof<IEventCreateValues>((x) => x.images),
                  images
                );
              }}
              setImagesTouched={async (value) => {
                await setFieldTouched(
                  nameof<IEventCreateValues>((x) => x.images),
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
                  nameof<IEventCreateValues>((x) => x.address),
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

export default EventCreate;
