import { Day } from "./day";

async function runDay(year: number, day: number) {
  console.log(`Running: ${year} - Day ${day}`);

  const importedDay = await import(
    `./${year}/day${String(day).padStart(2, "0")}/index`
  );

  const dayRunner: Day = importedDay.default;
  const partOneResult = await dayRunner.runPartOne();
  const partTwoResult = await dayRunner.runPartTwo();

  console.log(`Part 1: ${partOneResult}`);
  console.log(`Part 2: ${partTwoResult}`);
}

console.log("ADVENT OF CODE");
const params = process.argv.splice(2);
if (params.length) {
  runDay(parseInt(params[0]), parseInt(params[1]));
} else {
  console.log(`Usage: yarn start <year> <day>`);
}
