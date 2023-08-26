import { Formik } from "formik";
import { ISearchInputProps } from "./ISearchInputProps";
import styles from "./SearchInput.module.scss";
import { InitialValues } from "../../../../../common/utils/initialValues";
import { searchInputSchema } from "./SearchInputSchema";
import TextInput from "../../../../../components/text-input/TextInput";
import { Containers, InputTypes } from "../../../../../common/enums";
import { nameof } from "ts-simple-nameof";
import { IPostSearchQuery } from "../../../../../common/interfaces";
import { AiOutlineSearch } from "react-icons/ai";
import { useState } from "react";
import { TbFilterMinus, TbFilterPlus } from "react-icons/tb";
import BaseContainer from "../../../../../components/base-container/BaseContainer";
import { observer } from "mobx-react-lite";
import { useStore } from "../../../../../common/stores/store";
import { Checkbox } from "@chakra-ui/react";
import DateTimePicker from "../../../../../components/date-time-picker/DateTimePicker";
import OptionSelect from "../../../../../components/option-select/OptionSelect";

const SearchInput: React.FC<ISearchInputProps> = observer((props) => {
  const { onSearch } = props;

  const [filtersVisible, setFiltersVisible] = useState(false);

  const { commonStore } = useStore();

  const handleFiltersIconClick = () => {
    setFiltersVisible(!filtersVisible);
  };

  return (
    <BaseContainer type={Containers.Secondary} className={styles.container}>
      <Formik
        initialValues={{
          ...InitialValues.PostSearch,
          cityId: commonStore.cities[0].id,
        }}
        onSubmit={onSearch}
        validationSchema={searchInputSchema}
      >
        {({
          handleSubmit,
          submitForm,
          setFieldValue,
          values,
          errors,
          touched,
        }) => (
          <form onSubmit={handleSubmit} className={styles.formContainer}>
            <BaseContainer
              type={Containers.Secondary}
              className={styles.search}
            >
              <TextInput
                type={InputTypes.Text}
                errors={errors.query}
                touched={touched.query}
                placeholder="Search..."
                rightIcon={<AiOutlineSearch />}
                rightIconOnClick={submitForm}
                name={nameof<IPostSearchQuery>((x) => x.query)}
              />
              <div className={styles.icon} onClick={handleFiltersIconClick}>
                {filtersVisible ? <TbFilterMinus /> : <TbFilterPlus />}
              </div>
            </BaseContainer>
            {filtersVisible && (
              <div className={styles.hidden}>
                <div className={styles.categories}>
                  {commonStore.categories.map((category) => (
                    <Checkbox
                      key={category.id}
                      value={category.id}
                      className={styles.checkbox}
                      defaultChecked
                      onChange={(e) =>
                        setFieldValue(
                          nameof<IPostSearchQuery>((x) => x.skipCategoryIds),
                          e.target.checked
                            ? values.skipCategoryIds.filter(
                                (id) => id !== category.id
                              )
                            : [...values.skipCategoryIds, category.id]
                        )
                      }
                    >
                      {category.name}
                    </Checkbox>
                  ))}
                </div>
                <div className={styles.dates}>
                  <DateTimePicker
                    label="Start Date"
                    errors={errors.startDate as string}
                    touched={touched.startDate as boolean}
                    onChange={(date) => {
                      setFieldValue(
                        nameof<IPostSearchQuery>((x) => x.startDate),
                        date
                      );
                    }}
                  />
                  <DateTimePicker
                    label="End Date"
                    errors={errors.endDate as string}
                    touched={touched.endDate as boolean}
                    onChange={(date) =>
                      setFieldValue(
                        nameof<IPostSearchQuery>((x) => x.endDate),
                        date
                      )
                    }
                  />
                </div>
                <OptionSelect
                  label="City"
                  options={[
                    ...commonStore.cities.map((city) => ({
                      label: city.name,
                      value: city.id,
                    })),
                  ]}
                  name={nameof<IPostSearchQuery>((x) => x.cityId)}
                  errors={errors.cityId}
                  touched={touched.cityId}
                />
              </div>
            )}
          </form>
        )}
      </Formik>
    </BaseContainer>
  );
});

export default SearchInput;
