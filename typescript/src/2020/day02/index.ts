import { Day } from "../../day";

class Day02 implements Day {
  year = 2020;
  day = 2;
  name = "Password Philosophy";

  expectationsPartOne = [
    { input: "sample.txt", output: "2" },
    { input: "input.txt", output: "524" },
  ];

  partOne = (input: string): string => {
    const rows = input.split("\n");
    return rows.filter(validPasswordPartOne).length.toString();
  };

  expectationsPartTwo = [
    { input: "sample.txt", output: "1" },
    { input: "input.txt", output: "485" },
  ];

  partTwo = (input: string): string => {
    const rows = input.split("\n");
    return rows.filter(validPasswordPartTwo).length.toString();
  };
}

export default new Day02();

interface parsedRow {
  min: number;
  max: number;
  letter: string;
  password: string;
}

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
