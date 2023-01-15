import { Day, Expected } from "./day";
import { promises } from "fs";

async function runDay(year: number, day: number) {
  const dayRunner: Day = (
    await import(`./${year}/day${String(day).padStart(2, "0")}/index`)
  ).default;

  console.log(`--- ${year} - Day ${day}: ${dayRunner.name} ---`);
  console.log();

  await runPart(
    "Part 1",
    dayRunner,
    dayRunner.expectationsPartOne,
    dayRunner.partOne
  );
  await runPart(
    "Part 2",
    dayRunner,
    dayRunner.expectationsPartTwo,
    dayRunner.partTwo
  );

  console.log();
}

const runPart = async (
  description: string,
  day: Day,
  expectations: Expected[],
  run: (input: string) => string
) => {
  for (var expectiation of expectations) {
    const content = await promises.readFile(
      `./src/${day.year}/day${String(day.day).padStart(2, "0")}/${
        expectiation.input
      }`
    );

    const start = new Date().getTime();
    const result = run(content.toString());
    const elapsed = new Date().getTime() - start;

    if (result === expectiation.output) {
      console.log(
        `\x1B[32m✅ ${description}: (${expectiation.input}) ${result} ${elapsed}ms\x1B[0m`
      );
    } else {
      console.log(
        `\x1B[31m❌ ${description}: (${expectiation.input}) ${result} ${elapsed}ms (Expected: ${expectiation.output})\x1B[0m`
      );
    }
  }
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
