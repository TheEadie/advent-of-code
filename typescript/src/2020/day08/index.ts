import { Day } from "../../day";

class Day08 implements Day {
  year = 2020;
  day = 8;
  name = "Handheld Halting";

  expectationsPartOne = [
    { input: "sample.txt", output: "5" },
    { input: "input.txt", output: "1797" },
  ];

  partOne = (input: string): string => {
    const lines = input.split("\n");
    const instructions = lines.map(parse);
    return run(instructions).accumulator.toString();
  };

  expectationsPartTwo = [
    { input: "sample.txt", output: "8" },
    { input: "input.txt", output: "1036" },
  ];

  partTwo = (input: string): string => {
    const lines = input.split("\n");
    const instructions = lines.map(parse);

    const mutatedInstructions = instructions
      .map((instruction, i, original) => ({
        instruction,
        i,
        original,
      }))
      .filter(
        ({ instruction }) =>
          instruction.command === "nop" || instruction.command === "jmp"
      )
      .map(({ i, original }) => {
        const newInstructions = [...original];
        newInstructions[i] = swap(newInstructions[i]);
        return newInstructions;
      });

    const workingInstructions = mutatedInstructions.find(
      (x) => run(x).terminated
    );
    return run(workingInstructions).accumulator.toString();
  };
}

export default new Day08();

const parse = (input: string): instruction => {
  const lineRegex = input.match(/^(nop|acc|jmp) ([+|-][0-9]+)/);

  return {
    command: lineRegex[1],
    number: parseInt(lineRegex[2]),
  };
};

interface instruction {
  command: string;
  number: number;
}

interface programReturn {
  accumulator: number;
  terminated: boolean;
}

const run = (instructions: instruction[]): programReturn => {
  let accumulator = 0;
  let pointer = 0;
  const history: number[] = [];

  while (instructions[pointer] != undefined && !history.includes(pointer)) {
    const instruction = instructions[pointer];
    history.push(pointer);

    switch (instruction.command) {
      case "nop": {
        pointer++;
        break;
      }
      case "acc": {
        accumulator += instruction.number;
        pointer++;
        break;
      }
      case "jmp": {
        pointer += instruction.number;
        break;
      }
    }
  }

  if (instructions[pointer] === undefined) {
    return { accumulator: accumulator, terminated: true };
  }

  return { accumulator: accumulator, terminated: false };
};

const swap = (input: instruction): instruction => {
  if (input.command == "nop") {
    return { command: "jmp", number: input.number };
  }
  if (input.command == "jmp") {
    return { command: "nop", number: input.number };
  }
  return input;
};
