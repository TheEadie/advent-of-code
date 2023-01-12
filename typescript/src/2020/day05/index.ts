import { Day, Expected } from "../../day";

class Day05 extends Day {
  constructor() {
    super(2020, 5, "Binary Boarding");
  }

  expectationsPartOne = (): Expected[] => {
    return [{ input: "input.txt", output: "922" }];
  };

  partOne = (input: string): string => {
    const lines = input.split("\n");
    return Math.max(...lines.map(getSeatId)).toString();
  };

  expectationsPartTwo = (): Expected[] => {
    return [{ input: "input.txt", output: "747" }];
  };

  partTwo = (input: string): string => {
    const lines = input.split("\n");
    return lines
      .map(getSeatId)
      .sort((a, b) => a - b)
      .reduce((acc, val, ind, arr) => {
        if (val != arr[0] && val - arr[ind - 1] != 1) acc = val - 1;
        return acc;
      })
      .toString();
  };
}

export default new Day05();

const getSeatId = (input: string): number => {
  const row = getRow(input);
  const col = getCol(input);
  return row * 8 + col;
};

const getRow = (input: string): number => {
  const binary = input.substring(0, 7).replace(/F/g, "0").replace(/B/g, "1");
  return parseInt(binary, 2);
};

const getCol = (input: string): number => {
  const binary = input.substring(7, 10).replace(/L/g, "0").replace(/R/g, "1");
  return parseInt(binary, 2);
};
