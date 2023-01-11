import { Day } from "../../day";

class Day01 extends Day {
  constructor() {
    super(2020, 1);
  }

  partOne(input: string): string {
    const numbers = input.split("\n").map((x) => parseInt(x));

    for (let i = 0; i < numbers.length; i++) {
      const candidate = 2020 - numbers[i];
      if (numbers.includes(candidate)) {
        return (numbers[i] * candidate).toString();
      }
    }
  }

  partTwo(input: string): string {
    const numbers = input.split("\n").map((x) => parseInt(x));

    for (let i = 0; i < numbers.length; i++) {
      for (let j = 0; j < numbers.length; j++) {
        const candidate = 2020 - numbers[i] - numbers[j];
        if (numbers.includes(candidate)) {
          return (numbers[i] * numbers[j] * candidate).toString();
        }
      }
    }
  }
}

export default new Day01();
