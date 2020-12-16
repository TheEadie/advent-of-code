import { readFileSync } from "fs";

const file = readFileSync("./src/day02/input.txt", "utf-8");

const partOne = (input: string): number => {
  const rows = input.split("\n");
  return rows.filter(validPasswordPartOne).length;
};

const validPasswordPartOne = (input: string): boolean => {
  const [min, max, letter, password] = parseRow(input);

  const letterTally = password.split("").reduce((acc, char) => {
    acc[char] = (acc[char] || 0) + 1;
    return acc;
  }, {});

  const count = letterTally[letter];

  if (count >= min && count <= max) {
    return true;
  }

  return false;
};

const partTwo = (input: string): number => {
  const rows = input.split("\n");
  return rows.filter(validPasswordPartTwo).length;
};

const validPasswordPartTwo = (input: string): boolean => {
  const [lower, higher, letter, password] = parseRow(input);

  const passwordLetters = password.split("");
  const firstLetterIndex = lower - 1;
  const secondLetterIndex = higher - 1;

  if (
    (passwordLetters[firstLetterIndex] === letter) !=
    (passwordLetters[secondLetterIndex] === letter)
  ) {
    return true;
  }

  return false;
};

const parseRow = (
  input: string
): [min: number, max: number, letter: string, password: string] => {
  const elements = input.split(" ");

  const rangeString = elements[0];
  const charString = elements[1];
  const password = elements[2];

  const rangeElements = rangeString.split("-");
  const min = parseInt(rangeElements[0]);
  const max = parseInt(rangeElements[1]);

  const charElements = charString.split(":");
  const char = charElements[0];

  return [min, max, char, password];
};

console.log(`Part 1: ${partOne(file)}`);
console.log(`Part 2: ${partTwo(file)}`);
