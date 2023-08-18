import { Coordinate, Grid2D, eightDirections } from "../../utils/Space2D";
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
      _: Grid2D<boolean>
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
      map: Grid2D<boolean>
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

const run = (
  input: string,
  getNeighbours: (s: Coordinate, m: Grid2D<boolean>) => Coordinate[],
  maxOccupiedNeighbours: number
): string => {
  const seats = parseInput(input);
  let current = new Grid2D<boolean>([], false);
  let next = new Grid2D<boolean>(seats, false);

  const neighbours = new Map<Coordinate, Coordinate[]>();
  for (const seat of seats) {
    neighbours.set(seat, getNeighbours(seat, next));
  }

  while (!current.equals(next)) {
    current = next;
    next = runStep(current, neighbours, maxOccupiedNeighbours);
  }

  return next
    .getCellsWithValues()
    .filter((x) => next.getValue(x))
    .length.toString();
};

const runStep = (
  waitingArea: Grid2D<boolean>,
  neighboursForSeat: Map<Coordinate, Coordinate[]>,
  maxOccupiedNeighbours: number
): Grid2D<boolean> => {
  const next = new Grid2D<boolean>(waitingArea.getCellsWithValues(), false);
  for (const seat of waitingArea.getCellsWithValues()) {
    const isOccupied = waitingArea.getValue(seat);
    const occupiedNeighbours = neighboursForSeat
      .get(seat)
      .filter((x) => waitingArea.getValue(x)).length;
    if (!isOccupied && occupiedNeighbours === 0) {
      next.setValue(seat, true);
    } else if (isOccupied && occupiedNeighbours >= maxOccupiedNeighbours) {
      next.setValue(seat, false);
    } else {
      next.setValue(seat, isOccupied);
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
