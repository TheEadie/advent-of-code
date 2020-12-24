import { readFileSync } from "fs";

const file = readFileSync("./src/day08/input.txt", "utf-8").split("\n");

const partOne = (input: string[]): number => {
  const instructions = input.map(parse);
  return run(instructions).accumulator;
};

const partTwo = (input: string[]): number => {
  const instructions = input.map(parse);

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
  return run(workingInstructions).accumulator;
};

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

console.log(`Part 1: ${partOne(file)}`);
console.log(`Part 2: ${partTwo(file)}`);
