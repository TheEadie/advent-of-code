import { readFileSync } from "fs";

const file = readFileSync("./src/day04/input.txt", "utf-8");

interface passport {
  byr: number;
  iyr: number;
  eyr: number;
  hgt: string;
  hcl: string;
  ecl: eyeColour;
  pid: string;
  cid: string;
}

enum eyeColour {
  amb,
  blu,
  brn,
  gry,
  grn,
  hzl,
  oth,
}

const partOne = (input: string): number => {
  const rawPassports = input.split("\n\n");
  const passports = rawPassports.map(parsePassport);
  return passports.filter(validPassportPartOne).length;
};

const partTwo = (input: string): number => {
  const rawPassports = input.split("\n\n");
  const passports = rawPassports.map(parsePassport);
  return passports.filter(validPassportPartTwo).length;
};

const parsePassport = (input: string): passport => {
  const elements = input.split(/ |\n/);
  return {
    byr: parseYear(findItem(elements, "byr")),
    iyr: parseYear(findItem(elements, "iyr")),
    eyr: parseYear(findItem(elements, "eyr")),
    hgt: findItem(elements, "hgt"),
    hcl: findItem(elements, "hcl"),
    ecl: eyeColour[findItem(elements, "ecl")],
    pid: findItem(elements, "pid"),
    cid: findItem(elements, "cid"),
  };
};

const findItem = (input: string[], key: string): string | undefined => {
  let value = undefined;
  const found = input.find((x) => x.startsWith(key));

  if (found != undefined) {
    value = found.split(":")[1];
  }

  return value;
};

const parseYear = (input: string | undefined): number | undefined => {
  if (input === undefined) return undefined;
  return parseInt(input);
};

const validPassportPartOne = (passport: passport): boolean => {
  return (
    passport.byr != undefined &&
    passport.iyr != undefined &&
    passport.eyr != undefined &&
    passport.hgt != undefined &&
    passport.hcl != undefined &&
    passport.ecl != undefined &&
    passport.pid != undefined
  );
};

const validPassportPartTwo = (passport: passport): boolean => {
  return (
    passport.byr != undefined &&
    passport.byr >= 1920 &&
    passport.byr <= 2002 &&
    passport.iyr != undefined &&
    passport.iyr >= 2010 &&
    passport.iyr <= 2020 &&
    passport.eyr != undefined &&
    passport.eyr >= 2020 &&
    passport.eyr <= 2030 &&
    passport.hgt != undefined &&
    passport.hgt.match(/^(1([5-8][0-9]|9[0-3]))cm|(59|6[0-9]|7[0-6])in$/) &&
    passport.hcl != undefined &&
    passport.hcl.match(/^#[0-9a-f]{6}$/) &&
    passport.ecl != undefined &&
    passport.pid != undefined &&
    passport.pid.match(/^[0-9]{9}$/) &&
    true
  );
};

console.log(`Part 1: ${partOne(file)}`);
console.log(`Part 2: ${partTwo(file)}`);
