import { readFileSync } from "fs";

const file = readFileSync("./src/day01/input.txt", "utf-8");

const partOne = (input: string): number => {
  const numbers = input.split("\n").map((x) => parseInt(x));

  for (let i = 0; i < numbers.length; i++) {
    const candidate = 2020 - numbers[i];
    if (numbers.includes(candidate)) {
      return numbers[i] * candidate;
    }
  }
};

const partTwo = (input: string): number => {
  const numbers = input.split("\n").map((x) => parseInt(x));

  for (let i = 0; i < numbers.length; i++) {
    for (let j = 0; j < numbers.length; j++) {
      const candidate = 2020 - numbers[i] - numbers[j];
      if (numbers.includes(candidate)) {
        return numbers[i] * numbers[j] * candidate;
      }
    }
  }
};

console.log(`Part 1: ${partOne(file)}`);
console.log(`Part 2: ${partTwo(file)}`);
