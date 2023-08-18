class Grid2D<T> {
  private readonly values: Map<string, { key: Coordinate; value: T }>;
  private readonly maxX: number;
  private readonly maxY: number;

  constructor(cells: Coordinate[], initialState: T) {
    this.values = new Map<string, { key: Coordinate; value: T }>();

    for (const cell of cells) {
      this.setValue(cell, initialState);
    }

    this.maxX = Math.max(...[...cells].map((s) => s.x));
    this.maxY = Math.max(...[...cells].map((s) => s.y));
  }

  public isInBounds = (location: Coordinate): boolean =>
    location.x >= 0 &&
    location.y >= 0 &&
    location.x <= this.maxX &&
    location.y <= this.maxY;

  public getValue = (location: Coordinate): T | undefined =>
    this.values.get(this.getKey(location))?.value ?? undefined;

  public setValue = (location: Coordinate, value: T): void => {
    this.values.set(this.getKey(location), { key: location, value: value });
  };

  public hasValue = (location: Coordinate): boolean =>
    this.values.has(this.getKey(location));

  public getCellsWithValues = (): Coordinate[] =>
    [...this.values.keys()].map((key) => this.values.get(key).key);

  public equals = (other: Grid2D<T>): boolean => {
    return (
      this.values.size === other.values.size &&
      [...this.values.keys()].every(
        (value) =>
          other.values.get(value).value === this.values.get(value).value
      )
    );
  };

  private getKey = (coordinate: Coordinate): string =>
    `${coordinate.x},${coordinate.y}`;
}

type Coordinate = { x: number; y: number };
type Vector = { x: number; y: number };

const north: Vector = { x: 0, y: -1 };
const south: Vector = { x: 0, y: 1 };
const east: Vector = { x: 1, y: 0 };
const west: Vector = { x: -1, y: 0 };
const northEast: Vector = { x: 1, y: -1 };
const northWest: Vector = { x: -1, y: -1 };
const southEast: Vector = { x: 1, y: 1 };
const southWest: Vector = { x: -1, y: 1 };

const eightDirections: Vector[] = [
  north,
  south,
  east,
  west,
  northEast,
  northWest,
  southEast,
  southWest,
];

const fourDirections: Vector[] = [north, south, east, west];

export { Grid2D, Coordinate, Vector, eightDirections, fourDirections };
