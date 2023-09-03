import {
  FormControl,
  FormErrorMessage,
  FormLabel,
  useColorMode,
} from "@chakra-ui/react";
import Select, { GroupBase, StylesConfig } from "react-select";
import { ColorModes } from "../../common/enums";
import colors from "../../assets/styles/colors.module.scss";
import variables from "../../assets/styles/variables.module.scss";
import { IMultiOptionSelectProps } from "./IMultiOptionSelect";
import { observer } from "mobx-react-lite";
import Constants from "../../common/utils/constants";
import { IOption } from "../../common/interfaces";
import { importantStyle } from "../../common/utils/helpers";

const MultiOptionSelect: React.FC<IMultiOptionSelectProps> = observer(
  (props) => {
    const { onChange, name, label, options, errors, touched, isRequired } =
      props;

    const { colorMode } = useColorMode();

    const isLightMode = colorMode === ColorModes.Light;

    const styles: StylesConfig<IOption, true, GroupBase<IOption>> = {
      control: (styles) => ({
        ...styles,
        backgroundColor: isLightMode ? colors.primaryLight : colors.primaryDark,
        border: isLightMode ? colors.inputBorderLight : colors.inputBorderDark,
      }),
      valueContainer: (styles) => ({
        ...styles,
        " input:first-of-type": {
          border: importantStyle(Constants.CSS.None),
        },
      }),
      option: (styles) => ({
        ...styles,
        backgroundColor: isLightMode ? colors.primaryLight : colors.primaryDark,
        ":hover": {
          opacity: variables.hoverOpacity,
          filter: isLightMode
            ? variables.lessBrightness
            : variables.moreBrightness,
        },
      }),
      menu: (styles) => ({
        ...styles,
        backgroundColor: isLightMode ? colors.primaryLight : colors.primaryDark,
      }),
      multiValue: (styles) => ({
        ...styles,
        backgroundColor: Constants.CSS.Inherit,
        color: isLightMode ? colors.textLight : colors.textDark,
      }),
      multiValueLabel: (styles) => ({
        ...styles,
        color: isLightMode ? colors.textLight : colors.textDark,
      }),
      multiValueRemove: (styles) => ({
        ...styles,
        ":hover": {
          backgroundColor: Constants.CSS.Inherit,
          color: isLightMode ? colors.textLight : colors.textDark,
          opacity: variables.hoverOpacity,
        },
      }),
      indicatorSeparator: (styles) => ({
        ...styles,
        display: Constants.CSS.None,
      }),
      indicatorsContainer: (styles) => ({
        ...styles,
        " > div": {
          color: isLightMode ? colors.textLight : colors.textDark,
        },
      }),
    };

    return (
      <div>
        <FormControl isInvalid={!!errors && !!touched} isRequired={isRequired}>
          <FormLabel htmlFor={name}>{label}</FormLabel>
          <Select
            closeMenuOnSelect={false}
            isMulti
            placeholder=""
            styles={styles}
            onChange={(values) => onChange(values.map((x) => x.value))}
            name={name}
            options={options}
          />
          <FormErrorMessage>{errors}</FormErrorMessage>
        </FormControl>
      </div>
    );
  }
);

export default MultiOptionSelect;
