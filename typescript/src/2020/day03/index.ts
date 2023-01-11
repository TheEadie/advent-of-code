import { readFileSync } from "fs";

const file = readFileSync("./src/2020/day03/input.txt", "utf-8").split("\n");

const partOne = (input: string[]): number => {
  return countTreesHit(input, 3, 1);
};

const partTwo = (input: string[]): number => {
  return (
    countTreesHit(input, 3, 1) *
    countTreesHit(input, 1, 1) *
    countTreesHit(input, 5, 1) *
    countTreesHit(input, 7, 1) *
    countTreesHit(input, 1, 2)
  );
};

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

  console.log(`${treeCount}`);
  return treeCount;
};

console.log(`Part 1: ${partOne(file)}`);
console.log(`Part 2: ${partTwo(file)}`);
