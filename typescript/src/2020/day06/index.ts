import { readFileSync } from "fs";

const file = readFileSync("./src/2020/day06/input.txt", "utf-8").split("\n\n");

const partOne = (input: string[]): number => {
  return input.map(atLeastOneAnsweredYes).reduce((acc, val) => {
    acc += val;
    return acc;
  });
};

const partTwo = (input: string[]): number => {
  return input.map(allAnsweredYes).reduce((acc, val) => {
    acc += val;
    return acc;
  });
};

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

console.log(`Part 1: ${partOne(file)}`);
console.log(`Part 2: ${partTwo(file)}`);
