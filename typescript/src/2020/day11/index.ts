import { Day } from "../../day";

class Day11 implements Day {
  year = 2020;
  day = 11;
  name = "Seating System";

  expectationsPartOne = [
    { input: "sample.txt", output: "37" },
    { input: "input.txt", output: "2251" },
  ];

  partOne = (input: string): string => {
    const seats = parseInput(input);
    let current = new WaitingArea([]);
    let next = new WaitingArea(seats);

    const neighbours = new Map<string, Coordinate[]>();
    for (const seat of seats) {
      neighbours.set(`${seat[0]},${seat[1]}`, next.getNeighbours(seat));
    }

    while (!current.equals(next)) {
      current = next;
      next = runStep(current, neighbours, 4);
    }

    return next
      .getSeats()
      .filter((x) => next.isOccupied(x))
      .length.toString();
  };

  expectationsPartTwo = [
    { input: "sample.txt", output: "26" },
    { input: "input.txt", output: "2019" },
  ];

  partTwo = (input: string): string => {
    const seats = parseInput(input);
    let current = new WaitingArea([]);
    let next = new WaitingArea(seats);

    const neighbours = new Map<string, Coordinate[]>();
    for (const seat of seats) {
      neighbours.set(`${seat[0]},${seat[1]}`, next.getVisibleNeighbours(seat));
    }

    while (!current.equals(next)) {
      current = next;
      next = runStep(current, neighbours, 5);
    }

    return next
      .getSeats()
      .filter((x) => next.isOccupied(x))
      .length.toString();
  };
}

export default new Day11();

class WaitingArea {
  private readonly seats: Coordinate[];
  private readonly occupied: Map<string, boolean>;
  private readonly maxX: number;
  private readonly maxY: number;

  constructor(seats: Coordinate[]) {
    this.seats = seats;
    this.occupied = new Map<string, boolean>();

    this.maxX = Math.max(...[...this.seats].map((x) => x[0]));
    this.maxY = Math.max(...[...this.seats].map((x) => x[1]));
  }

  public getSeats = (): Coordinate[] => {
    return this.seats;
  };

  public isSeat = (seat: Coordinate): boolean => {
    return (
      this.seats.filter((x) => x[0] === seat[0] && x[1] === seat[1]).length > 0
    );
  };

  public isOccupied = (seat: Coordinate): boolean => {
    const key = `${seat[0]},${seat[1]}`;
    return this.occupied.get(key) ?? false;
  };

  public setOccupied = (seat: Coordinate): void => {
    const key = `${seat[0]},${seat[1]}`;
    this.occupied.set(key, true);
  };

  public getVisibleNeighbours = (seat: Coordinate): Coordinate[] => {
    const directions = [
      [-1, -1],
      [-1, 0],
      [-1, 1],
      [0, -1],
      [0, 1],
      [1, -1],
      [1, 0],
      [1, 1],
    ];

    // Find the first seat in each direction
    const neighbours = directions
      .map(([dx, dy]) => {
        let [x, y] = seat;
        while (x >= 0 && y >= 0 && x <= this.maxX && y <= this.maxY) {
          x += dx;
          y += dy;
          const testSeat: Coordinate = [x, y];
          if (this.isSeat(testSeat)) {
            return testSeat;
          }
        }
      })
      .filter((x) => x !== undefined);
    return neighbours;
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
      this.seats.length === other.seats.length &&
      this.occupied.size === other.occupied.size &&
      [...this.occupied.keys()].every((value) => other.occupied.has(value))
    );
  };
}

type Coordinate = [X: number, Y: number];

const runStep = (
  waitingArea: WaitingArea,
  neighboursForSeat: Map<string, Coordinate[]>,
  maxOccupiedNeighbours: number
): WaitingArea => {
  const next = new WaitingArea(waitingArea.getSeats());
  for (const seat of waitingArea.getSeats()) {
    const isOccupied = waitingArea.isOccupied(seat);
    const occupiedNeighbours = neighboursForSeat
      .get(`${seat[0]},${seat[1]}`)
      .filter((x) => waitingArea.isOccupied(x)).length;

    if (!isOccupied && occupiedNeighbours === 0) {
      next.setOccupied(seat);
    } else if (isOccupied && occupiedNeighbours >= maxOccupiedNeighbours) {
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
