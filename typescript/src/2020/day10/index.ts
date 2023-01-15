import { defaultMaxListeners } from "events";
import { Day } from "../../day";

class Day10 implements Day {
  year = 2020;
  day = 10;
  name = "Adapter Array";

  expectationsPartOne = [
    { input: "sample-1.txt", output: "35" },
    { input: "sample-2.txt", output: "220" },
    { input: "input.txt", output: "3034" },
  ];

  partOne = (input: string): string => {
    const diffs = getAdapters(input).map((v, i, all) => v - all[i - 1]);

    const one = diffs.filter((x) => x === 1).length;
    const three = diffs.filter((x) => x === 3).length;

    return (one * three).toString();
  };

  expectationsPartTwo = [
    { input: "sample-1.txt", output: "8" },
    { input: "sample-2.txt", output: "19208" },
    { input: "input.txt", output: "259172170858496" },
  ];

  partTwo = (input: string): string => {
    return getAdapters(input)
      .map((v, i, all) => v - all[i - 1])
      .filter((_, i) => i > 0)
      .toString()
      .replace(/,/g, "")
      .split("3")
      .map((x) => x.length)
      .filter((x) => x > 0)
      .map((x) => {
        if (x === 4) return 7;
        else if (x === 3) return 4;
        else return x;
      })
      .reduce((a, x) => a * x)
      .toString();
  };
}

export default new Day10();

const getAdapters = (input: string): number[] => {
  const numbers = input
    .split("\n")
    .map((x) => Number(x))
    .sort((a, b) => (a > b ? 1 : -1));
  const outlet = 0;
  const builtIn = Math.max(...numbers) + 3;

  return [outlet, ...numbers, builtIn];
};
