import { readFileSync } from "fs";

const file = readFileSync("./src/2020/day02/input.txt", "utf-8");

interface parsedRow {
  min: number;
  max: number;
  letter: string;
  password: string;
}

const partOne = (input: string): number => {
  const rows = input.split("\n");
  return rows.filter(validPasswordPartOne).length;
};

const validPasswordPartOne = (input: string): boolean => {
  const parsedRow = parseRow(input);

  const letterTally = parsedRow.password.split("").reduce((acc, char) => {
    acc[char] = (acc[char] || 0) + 1;
    return acc;
  }, {});

  const count = letterTally[parsedRow.letter];

  if (count >= parsedRow.min && count <= parsedRow.max) {
    return true;
  }

  return false;
};

const partTwo = (input: string): number => {
  const rows = input.split("\n");
  return rows.filter(validPasswordPartTwo).length;
};

const validPasswordPartTwo = (input: string): boolean => {
  const parsedRow = parseRow(input);

  const passwordLetters = parsedRow.password.split("");
  const firstLetterIndex = parsedRow.min - 1;
  const secondLetterIndex = parsedRow.max - 1;

  if (
    (passwordLetters[firstLetterIndex] === parsedRow.letter) !=
    (passwordLetters[secondLetterIndex] === parsedRow.letter)
  ) {
    return true;
  }

  return false;
};

const parseRow = (input: string): parsedRow => {
  const elements = input.split(" ");

  const rangeString = elements[0];
  const charString = elements[1];
  const password = elements[2];

  const rangeElements = rangeString.split("-");
  const min = parseInt(rangeElements[0]);
  const max = parseInt(rangeElements[1]);

  const charElements = charString.split(":");
  const char = charElements[0];

  return { min: min, max: max, letter: char, password: password };
};

console.log(`Part 1: ${partOne(file)}`);
console.log(`Part 2: ${partTwo(file)}`);
