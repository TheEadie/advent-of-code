class SparseGrid2D<T> implements Iterable<Coordinate> {
  private readonly values: Map<string, { key: Coordinate; value: T }>;
  private minX: number = 0;
  private minY: number = 0;
  private maxX: number = 0;
  private maxY: number = 0;

  constructor() {
    this.values = new Map<string, { key: Coordinate; value: T }>();
  }

  [Symbol.iterator](): Iterator<Coordinate> {
    return new GridIterator(this.getCoordinatesWithValues());
  }

  public isInBounds = (location: Coordinate): boolean =>
    location.x >= this.minX &&
    location.y >= this.minY &&
    location.x <= this.maxX &&
    location.y <= this.maxY;

  public getValue = (location: Coordinate): T | undefined =>
    this.values.get(this.getKey(location))?.value ?? undefined;

  public setValue = (location: Coordinate, value: T): void => {
    this.values.set(this.getKey(location), { key: location, value: value });
    if (location.x < this.minX) this.minX = location.x;
    if (location.y < this.minY) this.minY = location.y;
    if (location.x > this.maxX) this.maxX = location.x;
    if (location.y > this.maxY) this.maxY = location.y;
  };

  public hasValue = (location: Coordinate): boolean =>
    this.values.has(this.getKey(location));

  public clone = (): SparseGrid2D<T> => {
    const clone = new SparseGrid2D<T>();
    for (const cell of this) {
      clone.setValue(cell, this.getValue(cell));
    }
    return clone;
  };

  public equals = (other: SparseGrid2D<T>): boolean => {
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

  private getCoordinatesWithValues = (): Coordinate[] =>
    [...this.values.values()].map((x) => x.key);
}

class GridIterator<T> implements Iterator<T> {
  private index: number;
  private done: boolean;
  private values: T[];

  constructor(values: T[]) {
    this.index = 0;
    this.done = false;
    this.values = values;
  }

  next(): IteratorResult<T, number | undefined> {
    if (this.done) {
      return { value: undefined, done: true };
    }
    if (this.index === this.values.length) {
      this.done = true;
      return { value: this.index, done: this.done };
    }
    const value = this.values[this.index];
    this.index++;
    return { value, done: false };
  }
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

export { SparseGrid2D, Coordinate, Vector, eightDirections, fourDirections };
