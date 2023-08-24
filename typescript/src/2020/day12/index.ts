import { Day } from "../../day";
import {
  Coordinate,
  move,
  rotate,
  manhattanDistance,
} from "../../utils/space2d";

class Day12 implements Day {
  year = 2020;
  day = 12;
  name = "Rain Risk";

  expectationsPartOne = [
    { input: "sample.txt", output: "25" },
    { input: "input.txt", output: "1319" },
  ];

  partOne = (input: string): string => {
    const instructions = parseInput(input);
    const boat: Coordinate = { x: 0, y: 0 };
    let waypoint = { x: 1, y: 0 };

    for (let instruction of instructions) {
      switch (instruction.action) {
        case Action.North:
          boat.y += instruction.value;
          break;
        case Action.South:
          boat.y -= instruction.value;
          break;
        case Action.East:
          boat.x += instruction.value;
          break;
        case Action.West:
          boat.x -= instruction.value;
          break;
        case Action.Left:
          waypoint = rotate(waypoint, { x: 0, y: 0 }, -instruction.value);
          break;
        case Action.Right:
          waypoint = rotate(waypoint, { x: 0, y: 0 }, instruction.value);
          break;
        case Action.Forward:
          boat.x += waypoint.x * instruction.value;
          boat.y += waypoint.y * instruction.value;
          break;
      }
    }
    return manhattanDistance(boat, { x: 0, y: 0 }).toString();
  };

  expectationsPartTwo = [
    { input: "sample.txt", output: "286" },
    { input: "input.txt", output: "62434" },
  ];

  partTwo = (input: string): string => {
    const instructions = parseInput(input);
    const boat: Coordinate = { x: 0, y: 0 };
    let waypoint: Coordinate = { x: 10, y: 1 };

    for (let instruction of instructions) {
      switch (instruction.action) {
        case Action.North:
          waypoint.y += instruction.value;
          break;
        case Action.South:
          waypoint.y -= instruction.value;
          break;
        case Action.East:
          waypoint.x += instruction.value;
          break;
        case Action.West:
          waypoint.x -= instruction.value;
          break;
        case Action.Left:
          waypoint = rotate(waypoint, { x: 0, y: 0 }, -instruction.value);
          break;
        case Action.Right:
          waypoint = rotate(waypoint, { x: 0, y: 0 }, instruction.value);
          break;
        case Action.Forward:
          boat.x += waypoint.x * instruction.value;
          boat.y += waypoint.y * instruction.value;
          break;
      }
    }
    return manhattanDistance(boat, { x: 0, y: 0 }).toString();
  };
}

export default new Day12();

enum Action {
  North = "N",
  South = "S",
  East = "E",
  West = "W",
  Left = "L",
  Right = "R",
  Forward = "F",
}

type Instruction = {
  action: Action;
  value: number;
};

const parseInput = (input: string): Instruction[] => {
  return input.split("\n").map((x) => {
    return { action: x[0] as Action, value: parseInt(x.substring(1)) };
  });
};
