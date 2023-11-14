import { Formik } from "formik";
import { ISearchInputProps } from "./ISearchInputProps";
import styles from "./SearchInput.module.scss";
import { InitialValues } from "../../../../../common/utils/initialValues";
import { searchInputSchema } from "./SearchInputSchema";
import TextInput from "../../../../../components/text-input/TextInput";
import { Containers, InputTypes } from "../../../../../common/enums";
import { nameof } from "ts-simple-nameof";
import { IOption, IEventSearchQuery } from "../../../../../common/interfaces";
import { AiOutlineSearch } from "react-icons/ai";
import { useState } from "react";
import { TbFilterMinus, TbFilterPlus } from "react-icons/tb";
import BaseContainer from "../../../../../components/base-container/BaseContainer";
import { observer } from "mobx-react-lite";
import { useStore } from "../../../../../common/stores/store";
import { Checkbox } from "@chakra-ui/react";
import DateTimePicker from "../../../../../components/date-time-picker/DateTimePicker";
import OptionSelect from "../../../../../components/option-select/OptionSelect";
import { EventFilters } from "../../../../../common/enums/EventFilters";
import { getSelectedCityId } from "../../../../../common/utils/helpers";
import Constants from "../../../../../common/utils/constants";

const sortByOptions: IOption[] = [
  {
    value: EventFilters.Newest,
    label: "Newest",
  },
  {
    value: EventFilters.Oldest,
    label: "Oldest",
  },
  {
    value: EventFilters.MostPopular,
    label: "Most popular",
  },
  {
    value: EventFilters.LeastPopular,
    label: "Least popular",
  },
];

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
          ...InitialValues.EventSearch,
          cityId: getSelectedCityId() ?? commonStore.cities[0].id,
          sortBy: sortByOptions[0].value as string,
          pageSize: Constants.EventPageSize,
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
                placeholder={Constants.Placeholders.Search}
                rightIcon={<AiOutlineSearch />}
                rightIconOnClick={submitForm}
                name={nameof<IEventSearchQuery>((x) => x.query)}
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
                      isChecked={!values.skipCategoryIds?.includes(category.id)}
                      onChange={(e) =>
                        setFieldValue(
                          nameof<IEventSearchQuery>((x) => x.skipCategoryIds),
                          e.target.checked
                            ? (values.skipCategoryIds ?? []).filter(
                                (id) => id !== category.id
                              )
                            : [...(values.skipCategoryIds ?? []), category.id]
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
                        nameof<IEventSearchQuery>((x) => x.startDate),
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
                        nameof<IEventSearchQuery>((x) => x.endDate),
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
                  name={nameof<IEventSearchQuery>((x) => x.cityId)}
                  errors={errors.cityId}
                  touched={touched.cityId}
                />
                <OptionSelect
                  label="Sort By"
                  options={sortByOptions}
                  name={nameof<IEventSearchQuery>((x) => x.sortBy)}
                  errors={errors.sortBy}
                  touched={touched.sortBy}
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
