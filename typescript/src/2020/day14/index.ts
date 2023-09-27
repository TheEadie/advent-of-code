import { Day } from "../../day";

class Day14 implements Day {
  year = 2020;
  day = 14;
  name = "Docking Data";

  expectationsPartOne = [
    { input: "sample-1.txt", output: "165" },
    { input: "input.txt", output: "11884151942312" },
  ];

  partOne = (input: string): string => {
    const instructions = parseInput(input);
    const memory = new Map<number, number>();

    for (const instruction of instructions) {
      const binary = instruction.memory.value.toString(2).padStart(36, "0");
      const masked = binary
        .split("")
        .map((x, i) => (instruction.mask[i] === "X" ? x : instruction.mask[i]))
        .join("");
      memory.set(instruction.memory.address, parseInt(masked, 2));
    }
    return [...memory.values()].reduce((acc, x) => acc + x, 0).toString();
  };

  expectationsPartTwo = [
    { input: "sample-2.txt", output: "208" },
    { input: "input.txt", output: "2625449018811" },
  ];

  partTwo = (input: string): string => {
    const instructions = parseInput(input);
    const memory = new Map<number, number>();

    for (const instruction of instructions) {
      const binary = instruction.memory.address.toString(2).padStart(36, "0");
      const masked = binary
        .split("")
        .map((x, i) => (instruction.mask[i] === "0" ? x : instruction.mask[i]))
        .join("");

      const floating = masked.split("").filter((x) => x === "X").length;
      const combinations = Math.pow(2, floating);

      for (let i = 0; i < combinations; i++) {
        const binary = i.toString(2).padStart(floating, "0");
        let address = masked;
        for (const bit of binary) {
          address = address.replace("X", bit);
        }
        memory.set(parseInt(address, 2), instruction.memory.value);
      }
    }

    return [...memory.values()].reduce((acc, x) => acc + x, 0).toString();
  };
}

export default new Day14();

type Instruction = { mask: string; memory: { address: number; value: number } };

const parseInput = (input: string): Instruction[] => {
  const lines = input.split("\n");
  let mask = "";
  const instructions: Instruction[] = [];

  for (const line of lines) {
    if (line.startsWith("mask")) {
      mask = line.split(" = ")[1];
    } else {
      const [left, right] = line.split(" = ");
      const address = parseInt(left.replace("mem[", "").replace("]", ""));
      const value = parseInt(right);
      instructions.push({ mask, memory: { address, value } });
    }
  }

  return instructions;
};
