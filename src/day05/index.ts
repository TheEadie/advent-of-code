import { readFileSync } from "fs";

const file = readFileSync("./src/day05/input.txt", "utf-8").split("\n");

const partOne = (input: string[]): number => {
  return Math.max(...input.map(getSeatId));
};

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

const partTwo = (input: string[]): number => {
  return input
    .map(getSeatId)
    .sort((a, b) => a - b)
    .reduce((acc, val, ind, arr) => {
      if (val != arr[0] && val - arr[ind - 1] != 1) acc = val - 1;
      return acc;
    });
};

console.log(`Part 1: ${partOne(file)}`);
console.log(`Part 2: ${partTwo(file)}`);
