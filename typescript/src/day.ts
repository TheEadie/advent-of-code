import { promises } from "fs";

abstract class Day {
  year: number;
  day: number;

  constructor(year: number, day: number) {
    this.year = year;
    this.day = day;
  }

  async runPartOne(): Promise<string> {
    const content = await promises.readFile(
      `./src/${this.year}/day${String(this.day).padStart(2, "0")}/input.txt`
    );
    const result = this.partOne(content.toString());
    return result;
  }

  abstract partOne(input: string): string;

  async runPartTwo(): Promise<string> {
    const content = await promises.readFile(
      `./src/${this.year}/day${String(this.day).padStart(2, "0")}/input.txt`
    );
    const result = this.partTwo(content.toString());
    return result;
  }

  abstract partTwo(input: string): string;
}

export { Day };
