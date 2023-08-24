const last = <T>(generator: Generator<T>): T => {
  let last: T;
  for (let value of generator) {
    last = value;
  }
  return last;
};

export { last };
