import { promises } from "fs";

abstract class Day {
  year: number;
  day: number;
  name: string;
  dayZeroPadded: string;

  constructor(year: number, day: number, name: string) {
    this.year = year;
    this.day = day;
    this.name = name;
    this.dayZeroPadded = String(day).padStart(2, "0");
  }

  abstract expectationsPartOne: () => Expected[];
  abstract expectationsPartTwo: () => Expected[];
  abstract partOne: (input: string) => string;
  abstract partTwo: (input: string) => string;

  runPartOne = async (fileName: string): Promise<string> => {
    const content = await promises.readFile(
      `./src/${this.year}/day${this.dayZeroPadded}/${fileName}`
    );
    const result = this.partOne(content.toString());
    return result;
  };

  runPartTwo = async (fileName: string): Promise<string> => {
    const content = await promises.readFile(
      `./src/${this.year}/day${this.dayZeroPadded}/${fileName}`
    );
    const result = this.partTwo(content.toString());
    return result;
  };
}

interface Expected {
  input: string;
  output: string;
}

export { Day, Expected };
