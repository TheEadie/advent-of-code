import { Day, Expected } from "../../day";

class Day06 extends Day {
  constructor() {
    super(2020, 6, "Custom Customs");
  }

  expectationsPartOne = (): Expected[] => {
    return [
      { input: "sample.txt", output: "11" },
      { input: "input.txt", output: "6549" },
    ];
  };

  partOne = (input: string): string => {
    const lines = input.split("\n\n");
    return lines
      .map(atLeastOneAnsweredYes)
      .reduce((acc, val) => {
        acc += val;
        return acc;
      })
      .toString();
  };

  expectationsPartTwo = (): Expected[] => {
    return [
      { input: "sample.txt", output: "6" },
      { input: "input.txt", output: "3466" },
    ];
  };

  partTwo = (input: string): string => {
    const lines = input.split("\n\n");
    return lines
      .map(allAnsweredYes)
      .reduce((acc, val) => {
        acc += val;
        return acc;
      })
      .toString();
  };
}

export default new Day06();

const atLeastOneAnsweredYes = (input: string): number => {
  const letterTally = input.split("").reduce((acc, char) => {
    if (char === "\n") return acc;
    acc[char] = (acc[char] || 0) + 1;
    return acc;
  }, {});

  return Object.keys(letterTally).length;
};

const allAnsweredYes = (input: string): number => {
  const numberOfPeople = input.split("\n").length;
  const letterTally = input.split("").reduce((acc, char) => {
    if (char === "\n") return acc;
    acc[char] = (acc[char] || 0) + 1;
    return acc;
  }, {});

  return Object.keys(letterTally).filter(
    (x) => letterTally[x] === numberOfPeople
  ).length;
};
