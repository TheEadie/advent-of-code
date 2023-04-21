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
    const getNeighbours = (
      { x, y }: Coordinate,
      _: WaitingArea
    ): Coordinate[] => {
      const deltas: Coordinate[] = [
        { x: -1, y: -1 },
        { x: -1, y: 0 },
        { x: -1, y: 1 },
        { x: 0, y: -1 },
        { x: 0, y: 1 },
        { x: 1, y: -1 },
        { x: 1, y: 0 },
        { x: 1, y: 1 },
      ];
      return deltas.map((d) => ({ x: x + d.x, y: y + d.y }));
    };

    return run(input, getNeighbours, 4);
  };

  expectationsPartTwo = [
    { input: "sample.txt", output: "26" },
    { input: "input.txt", output: "2019" },
  ];

  partTwo = (input: string): string => {
    const getNeighbours = (
      seat: Coordinate,
      map: WaitingArea
    ): Coordinate[] => {
      const directions: Coordinate[] = [
        { x: -1, y: -1 },
        { x: -1, y: 0 },
        { x: -1, y: 1 },
        { x: 0, y: -1 },
        { x: 0, y: 1 },
        { x: 1, y: -1 },
        { x: 1, y: 0 },
        { x: 1, y: 1 },
      ];

      // Find the first seat in each direction
      const neighbours = directions
        .map((d) => {
          let { x, y } = seat;
          while (x >= 0 && y >= 0 && x <= map.getMaxX() && y <= map.getMaxY()) {
            x += d.x;
            y += d.y;
            const testSeat = { x, y };
            if (map.isSeat(testSeat)) {
              return testSeat;
            }
          }
        })
        .filter((x) => x !== undefined);
      return neighbours;
    };

    return run(input, getNeighbours, 5);
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

    this.maxX = Math.max(...[...this.seats].map((s) => s.x));
    this.maxY = Math.max(...[...this.seats].map((s) => s.y));
  }

  public getMaxX = (): number => this.maxX;

  public getMaxY = (): number => this.maxY;

  public getSeats = (): Coordinate[] => this.seats;

  public isSeat = (location: Coordinate): boolean =>
    this.seats.filter((s) => s.x === location.x && s.y === location.y).length >
    0;

  public isOccupied = (location: Coordinate): boolean =>
    this.occupied.get(getKey(location)) ?? false;

  public setOccupied = (location: Coordinate): void => {
    this.occupied.set(getKey(location), true);
  };

  public equals = (other: WaitingArea): boolean => {
    return (
      this.seats.length === other.seats.length &&
      this.occupied.size === other.occupied.size &&
      [...this.occupied.keys()].every((value) => other.occupied.has(value))
    );
  };
}

type Coordinate = { x: number; y: number };

const getKey = (coordinate: Coordinate): string =>
  `${coordinate.x},${coordinate.y}`;

const run = (
  input: string,
  getNeighbours: (s: Coordinate, m: WaitingArea) => Coordinate[],
  maxOccupiedNeighbours: number
): string => {
  const seats = parseInput(input);
  let current = new WaitingArea([]);
  let next = new WaitingArea(seats);

  const neighbours = new Map<string, Coordinate[]>();
  for (const seat of seats) {
    neighbours.set(getKey(seat), getNeighbours(seat, next));
  }

  while (!current.equals(next)) {
    current = next;
    next = runStep(current, neighbours, maxOccupiedNeighbours);
  }

  return next
    .getSeats()
    .filter((x) => next.isOccupied(x))
    .length.toString();
};

const runStep = (
  waitingArea: WaitingArea,
  neighboursForSeat: Map<string, Coordinate[]>,
  maxOccupiedNeighbours: number
): WaitingArea => {
  const next = new WaitingArea(waitingArea.getSeats());
  for (const seat of waitingArea.getSeats()) {
    const isOccupied = waitingArea.isOccupied(seat);
    const occupiedNeighbours = neighboursForSeat
      .get(getKey(seat))
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
        seats.add({ x: x, y: y });
      }
    }
  }
  return [...seats];
};
