import { Day } from "../../day";

class Day01 implements Day {
  name = "Report Repair";
  year = 2020;
  day = 1;

  expectationsPartOne = [
    { input: "sample.txt", output: "514579" },
    { input: "input.txt", output: "326211" },
  ];

  partOne = (input: string): string => {
    const numbers = input.split("\n").map((x) => parseInt(x));

    for (let i = 0; i < numbers.length; i++) {
      const candidate = 2020 - numbers[i];
      if (numbers.includes(candidate)) {
        return (numbers[i] * candidate).toString();
      }
    }
  };

  expectationsPartTwo = [
    { input: "sample.txt", output: "241861950" },
    { input: "input.txt", output: "131347190" },
  ];

  partTwo = (input: string): string => {
    const numbers = input.split("\n").map((x) => parseInt(x));

    for (let i = 0; i < numbers.length; i++) {
      for (let j = 0; j < numbers.length; j++) {
        const candidate = 2020 - numbers[i] - numbers[j];
        if (numbers.includes(candidate)) {
          return (numbers[i] * numbers[j] * candidate).toString();
        }
      }
    }
  };
}

export default new Day01();
