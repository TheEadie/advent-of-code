import { Day } from "../../day";

class Day13 implements Day {
  year = 2020;
  day = 13;
  name = "Shuttle Search";

  expectationsPartOne = [
    { input: "sample.txt", output: "295" },
    { input: "input.txt", output: "3865" },
  ];

  partOne = (input: string): string => {
    const { start, buses } = parseInput(input);
    let t = start;
    while (true) {
      for (const bus of buses.filter((x) => !isNaN(x))) {
        if (t % bus === 0) {
          return ((t - start) * bus).toString();
        }
      }
      t++;
    }
  };

  expectationsPartTwo = [
    { input: "sample.txt", output: "1068781" },
    { input: "input.txt", output: "415579909629976" },
  ];

  partTwo = (input: string): string => {
    const { buses } = parseInput(input);
    const busInfo = buses
      .map((x, i) => ({ busId: x, offset: i }))
      .filter((x) => !isNaN(x.busId));

    let t = 0;
    let found = false;
    let step = busInfo[0].busId;
    while (!found) {
      t += step;

      const matches = busInfo
        .map((bus) => ({
          busId: bus.busId,
          found: (t + bus.offset) % bus.busId == 0,
        }))
        .filter((x) => x.found);

      step = matches.map((x) => x.busId).reduce((acc, id) => acc * id, 1);
      found = matches.length === busInfo.length;
    }
    return t.toString();
  };
}

export default new Day13();

type Input = { start: number; buses: number[] };

const parseInput = (input: string): Input => {
  const lines = input.split("\n");
  return {
    start: parseInt(lines[0]),
    buses: lines[1].split(",").map((x) => parseInt(x)),
  };
};
