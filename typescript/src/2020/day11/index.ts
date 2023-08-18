import { Coordinate, SparseGrid2D, eightDirections } from "../../utils/Space2D";
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
      _: SparseGrid2D<seatType>
    ): Coordinate[] => eightDirections.map((d) => ({ x: x + d.x, y: y + d.y }));

    return run(input, getNeighbours, 4);
  };

  expectationsPartTwo = [
    { input: "sample.txt", output: "26" },
    { input: "input.txt", output: "2019" },
  ];

  partTwo = (input: string): string => {
    const getNeighbours = (
      seat: Coordinate,
      map: SparseGrid2D<seatType>
    ): Coordinate[] =>
      eightDirections
        .map((d) => {
          let current = { ...seat };
          while (map.isInBounds(current)) {
            current.x += d.x;
            current.y += d.y;
            if (map.hasValue(current)) {
              return current;
            }
          }
        })
        .filter((x) => x !== undefined);

    return run(input, getNeighbours, 5);
  };
}

export default new Day11();

enum seatType {
  "Floor",
  "Empty",
  "Occupied",
}

const run = (
  input: string,
  getNeighbours: (s: Coordinate, m: SparseGrid2D<seatType>) => Coordinate[],
  maxOccupiedNeighbours: number
): string => {
  let current = new SparseGrid2D<seatType>(seatType.Floor);
  let next = parseInput(input);

  const neighbours = new Map<Coordinate, Coordinate[]>();
  for (const seat of next) {
    neighbours.set(seat, getNeighbours(seat, next));
  }

  while (!current.equals(next)) {
    current = next;
    next = runStep(current, neighbours, maxOccupiedNeighbours);
  }

  return Array.from(next)
    .filter((x) => next.getValue(x) === seatType.Occupied)
    .length.toString();
};

const runStep = (
  waitingArea: SparseGrid2D<seatType>,
  neighboursForSeat: Map<Coordinate, Coordinate[]>,
  maxOccupiedNeighbours: number
): SparseGrid2D<seatType> => {
  const next = waitingArea.clone();

  for (const seat of waitingArea) {
    const isOccupied = isSeatOccupied(seat, waitingArea);
    const occupiedNeighbours = neighboursForSeat
      .get(seat)
      .filter((x) => isSeatOccupied(x, waitingArea)).length;
    if (!isOccupied && occupiedNeighbours === 0) {
      next.setValue(seat, seatType.Occupied);
    } else if (isOccupied && occupiedNeighbours >= maxOccupiedNeighbours) {
      next.setValue(seat, seatType.Empty);
    } else {
      next.setValue(seat, isOccupied ? seatType.Occupied : seatType.Empty);
    }
  }

  return next;
};

const isSeatOccupied = (
  seat: Coordinate,
  map: SparseGrid2D<seatType>
): boolean => map.getValue(seat) === seatType.Occupied;

const parseInput = (input: string): SparseGrid2D<seatType> => {
  const seats = new SparseGrid2D<seatType>(seatType.Floor);
  const chars = input.split("\n").map((x) => x.split(""));

  for (let [y, row] of chars.entries()) {
    for (let [x, cell] of row.entries()) {
      if (cell === "L") {
        seats.setValue({ x: x, y: y }, seatType.Empty);
      }
    }
  }
  return seats;
};
