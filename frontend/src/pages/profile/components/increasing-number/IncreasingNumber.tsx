import { useState, useEffect } from "react";
import { IIncreasingNumberProps } from "./IIncreasingNumberProps";
import styles from "./IncreasingNumber.module.scss";
import Constants from "../../../../common/utils/constants";

const IncreasingNumber: React.FC<IIncreasingNumberProps> = (props) => {
  const {
    start = 0,
    number,
    duration = Constants.DefaultIncreaseInterval,
  } = props;
  const [currentNumber, setCurrentNumber] = useState(start);

  useEffect(() => {
    if (start >= number || number <= 1) {
      setCurrentNumber(number);
      return;
    }

    const delay = duration / (number - start);
    const interval = setInterval(() => {
      setCurrentNumber((prevNumber) => {
        if (prevNumber + 1 > number) {
          clearInterval(interval);
          return number;
        }
        return prevNumber + 1;
      });
    }, delay);

    return () => clearInterval(interval);
  }, [start, number, duration]);

  return <span className={styles.container}>{Math.round(currentNumber)}</span>;
};

export default IncreasingNumber;
