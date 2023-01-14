interface Day {
  name: string;
  year: number;
  day: number;

  expectationsPartOne: Expected[];
  expectationsPartTwo: Expected[];
  partOne: (input: string) => string;
  partTwo: (input: string) => string;
}

interface Expected {
  input: string;
  output: string;
}

export { Day, Expected };
