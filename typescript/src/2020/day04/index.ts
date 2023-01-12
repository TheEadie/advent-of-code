import { Day, Expected } from "../../day";

class Day04 extends Day {
  constructor() {
    super(2020, 4, "Passport Processing");
  }

  expectationsPartOne = (): Expected[] => {
    return [
      { input: "sample.txt", output: "2" },
      { input: "input.txt", output: "206" },
    ];
  };

  partOne = (input: string): string => {
    const rawPassports = input.split("\n\n");
    const passports = rawPassports.map(parsePassport);
    return passports.filter(validPassportPartOne).length.toString();
  };

  expectationsPartTwo = (): Expected[] => {
    return [{ input: "input.txt", output: "123" }];
  };

  partTwo = (input: string): string => {
    const rawPassports = input.split("\n\n");
    const passports = rawPassports.map(parsePassport);
    return passports.filter(validPassportPartTwo).length.toString();
  };
}

export default new Day04();

interface passport {
  byr: number;
  iyr: number;
  eyr: number;
  hgt: string;
  hcl: string;
  ecl: string;
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

const parsePassport = (input: string): passport => {
  const elements = input.split(/ |\n/);
  return {
    byr: parseYear(findItem(elements, "byr")),
    iyr: parseYear(findItem(elements, "iyr")),
    eyr: parseYear(findItem(elements, "eyr")),
    hgt: findItem(elements, "hgt"),
    hcl: findItem(elements, "hcl"),
    ecl: findItem(elements, "ecl"),
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
  var eyes = eyeColour[passport.ecl];
  return (
    eyes != undefined &&
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
