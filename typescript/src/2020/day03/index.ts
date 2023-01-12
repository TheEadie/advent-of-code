import { Day, Expected } from "../../day";

class Day03 extends Day {
  constructor() {
    super(2020, 3, "Toboggan Trajectory");
  }

  expectationsPartOne = (): Expected[] => {
    return [
      { input: "sample.txt", output: "7" },
      { input: "input.txt", output: "191" },
    ];
  };

  partOne = (input: string): string => {
    const rows = input.split("\n");
    return countTreesHit(rows, 3, 1).toString();
  };

  expectationsPartTwo = (): Expected[] => {
    return [
      { input: "sample.txt", output: "336" },
      { input: "input.txt", output: "1478615040" },
    ];
  };

  partTwo = (input: string): string => {
    const rows = input.split("\n");
    return (
      countTreesHit(rows, 3, 1) *
      countTreesHit(rows, 1, 1) *
      countTreesHit(rows, 5, 1) *
      countTreesHit(rows, 7, 1) *
      countTreesHit(rows, 1, 2)
    ).toString();
  };
}

export default new Day03();

const countTreesHit = (
  input: string[],
  right: number,
  down: number
): number => {
  let x = 0;
  let treeCount = 0;

  for (let y = 0; y < input.length; y += down) {
    const line = input[y];
    const maxX = line.length;
    const value = line.split("")[x % maxX];

    if (value === "#") {
      treeCount++;
    }

    x += right;
  }

  return treeCount;
};
