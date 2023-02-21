import { Day } from "../../day";

class Day11 implements Day {
  year = 2020;
  day = 11;
  name = "Adapter Array";

  expectationsPartOne = [
    { input: "sample.txt", output: "37" },
    { input: "input.txt", output: "2251" },
  ];

  partOne = (input: string): string => {
    const seats = parseInput(input);
    let current = new WaitingArea([]);
    let next = new WaitingArea(seats);

    while (!current.equals(next)) {
      current = next;
      next = step(current);
    }

    return next
      .getSeats()
      .filter((x) => next.isOccupied(x))
      .length.toString();
  };

  expectationsPartTwo = [
    { input: "sample-1.txt", output: "26" },
    //{ input: "input.txt", output: "259172170858496" },
  ];

  partTwo = (input: string): string => {
    return "";
  };
}

export default new Day11();

class WaitingArea {
  private readonly seats: Set<Coordinate>;
  private readonly occupied: Map<string, boolean>;

  constructor(seats: Coordinate[]) {
    this.seats = new Set<Coordinate>(seats);
    this.occupied = new Map<string, boolean>();
  }

  public getSeats = (): Coordinate[] => {
    return [...this.seats];
  };

  public isOccupied = (seat: Coordinate): boolean => {
    const key = `${seat[0]},${seat[1]}`;
    return this.occupied.get(key) ?? false;
  };

  public setOccupied = (seat: Coordinate): void => {
    const key = `${seat[0]},${seat[1]}`;
    this.occupied.set(key, true);
  };

  public getNeighbours = ([x, y]: Coordinate): Coordinate[] => {
    const deltas: Coordinate[] = [
      [-1, -1],
      [-1, 0],
      [-1, 1],
      [0, -1],
      [0, 1],
      [1, -1],
      [1, 0],
      [1, 1],
    ];
    return deltas.map(([dx, dy]) => [x + dx, y + dy]);
  };

  public equals = (other: WaitingArea): boolean => {
    return (
      this.seats.size === other.seats.size &&
      this.occupied.size === other.occupied.size &&
      [...this.occupied.keys()].every((value) => other.occupied.has(value))
    );
  };
}

type Coordinate = [X: number, Y: number];

const numberOfOccupiedNeighbours = (
  waitingArea: WaitingArea,
  seat: Coordinate
): number => {
  return waitingArea
    .getNeighbours(seat)
    .filter((x) => waitingArea.isOccupied(x)).length;
};

const step = (waitingArea: WaitingArea): WaitingArea => {
  const next = new WaitingArea(waitingArea.getSeats());
  for (const seat of waitingArea.getSeats()) {
    const isOccupied = waitingArea.isOccupied(seat);
    const occupiedNeighbours = numberOfOccupiedNeighbours(waitingArea, seat);
    if (!isOccupied && occupiedNeighbours === 0) {
      next.setOccupied(seat);
    } else if (isOccupied && occupiedNeighbours >= 4) {
      // They leave
    } else if (isOccupied) {
      next.setOccupied(seat);
    }
  }
  return next;
};

const parseInput = (input: string): Coordinate[] => {
  const seats = new Set<Coordinate>();
  const chars = input.split("\n").map((x) => x.split(""));

  for (let [y, row] of chars.entries()) {
    for (let [x, cell] of row.entries()) {
      if (cell === "L") {
        seats.add([x, y]);
      }
    }
  }
  return [...seats];
};

function toString(area: WaitingArea): string {
  let str = "";
  for (let y = 0; y < 10; y++) {
    for (let x = 0; x < 10; x++) {
      str += area.isOccupied([x, y]) ? "X" : ".";
    }
    str += "\n";
  }
  return str;
}
