import { Day } from "./day";
import { promises } from "fs";

async function runDay(year: number, day: number) {
  const dayRunner: Day = (
    await import(`./${year}/day${String(day).padStart(2, "0")}/index`)
  ).default;

  console.log(`--- ${year} - Day ${day}: ${dayRunner.name} ---`);
  console.log();

  for (var expectiation of dayRunner.expectationsPartOne) {
    const start = new Date().getTime();
    const partOneResult = await runPartOne(dayRunner, expectiation.input);
    const elapsed = new Date().getTime() - start;

    if (partOneResult === expectiation.output) {
      console.log(
        `\x1B[32m✅ Part 1: (${expectiation.input}) ${partOneResult} ${elapsed}ms\x1B[0m`
      );
    } else {
      console.log(
        `\x1B[31m❌ Part 1: (${expectiation.input}) ${partOneResult} ${elapsed}ms (Expected: ${expectiation.output})\x1B[0m`
      );
    }
  }

  for (var expectiation of dayRunner.expectationsPartTwo) {
    const start = new Date().getTime();
    const result = await runPartTwo(dayRunner, expectiation.input);
    const elapsed = new Date().getTime() - start;

    if (result === expectiation.output) {
      console.log(
        `\x1B[32m✅ Part 2: (${expectiation.input}) ${result} ${elapsed}ms\x1B[0m`
      );
    } else {
      console.log(
        `\x1B[31m❌ Part 2: (${expectiation.input}) ${result} ${elapsed}ms (Expected: ${expectiation.output})\x1B[0m`
      );
    }
  }

  console.log();
}

const runPartOne = async (day: Day, fileName: string): Promise<string> => {
  const content = await promises.readFile(
    `./src/${day.year}/day${String(day.day).padStart(2, "0")}/${fileName}`
  );
  const result = day.partOne(content.toString());
  return result;
};

const runPartTwo = async (day: Day, fileName: string): Promise<string> => {
  const content = await promises.readFile(
    `./src/${day.year}/day${String(day.day).padStart(2, "0")}/${fileName}`
  );
  const result = day.partTwo(content.toString());
  return result;
};

async function run() {
  console.log("🎄🎄🎄   ADVENT OF CODE   🎄🎄🎄");
  const params = process.argv.splice(2);
  if (params.length === 2) {
    runDay(parseInt(params[0]), parseInt(params[1]));
  } else if (params.length === 1) {
    for (let i = 1; i <= 9; i++) {
      await runDay(parseInt(params[0]), i);
    }
  } else {
    console.log(`Usage: yarn start <year> <day>`);
  }
}

run();
