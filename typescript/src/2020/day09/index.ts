import { Day, Expected } from "../../day";

class Day09 extends Day {
  constructor() {
    super(2020, 9, "Encoding Error");
  }

  expectationsPartOne = (): Expected[] => {
    return [
      { input: "sample.txt", output: "127" },
      { input: "input.txt", output: "400480901" },
    ];
  };

  partOne = (input: string): string => {
    const numbers = input.split("\n").map((x) => parseInt(x));
    const numToLookback = numbers.length > 20 ? 25 : 5;
    return findWeakness(numbers, numToLookback).toString();
  };

  expectationsPartTwo = (): Expected[] => {
    return [
      { input: "sample.txt", output: "62" },
      { input: "input.txt", output: "67587168" },
    ];
  };

  partTwo = (input: string): string => {
    const numbers = input.split("\n").map((x) => parseInt(x));
    const numToLookback = numbers.length > 20 ? 25 : 5;
    const weakness = findWeakness(numbers, numToLookback);

    for (let [i, _] of numbers.entries()) {
      let list = new Set<number>();

      for (let next of numbers.slice(i, numbers.length)) {
        list.add(next);
        const currentTotal = [...list].reduce((a, x) => (a += x));

        if (currentTotal > weakness) {
          break;
        }
        if (currentTotal === weakness) {
          const smallest = Math.min(...list);
          const biggest = Math.max(...list);
          return (smallest + biggest).toString();
        }
      }
    }
  };
}

export default new Day09();

const findWeakness = (numbers: number[], numToLookback: number): number => {
  for (let i = numToLookback; i < numbers.length; i++) {
    const number = numbers[i];
    const lookback = numbers.slice(i - numToLookback, i);
    if (!containsTwoNumbersThatSum(number, lookback)) {
      return number;
    }
  }
};

const containsTwoNumbersThatSum = (
  number: number,
  lookup: number[]
): boolean => {
  for (let j = 0; j < lookup.length; j++) {
    const candidate = number - lookup[j];
    if (lookup.includes(candidate)) {
      return true;
    }
  }

  return false;
};
