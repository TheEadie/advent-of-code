import { readFileSync } from "fs";

const file = readFileSync("./src/day07/input.txt", "utf-8").split("\n");

const partOne = (input: string[]): number => {
  const bags = input.map(parse);
  const bagGraph = createGraph(bags);
  const goldBag = bagGraph.find((x) => x.colour === "shiny gold");

  const result = [];
  getBagColoursBagCanBeIn(result, goldBag);
  const resultSet = new Set(result);

  return resultSet.size - 1;
};

const partTwo = (input: string[]): number => {
  const bags = input.map(parse);
  const result = numberOfBags("shiny gold", bags);
  return result - 1;
};

const getBagColoursBagCanBeIn = (colours: string[], bag: bagNode) => {
  colours.push(bag.colour);
  bag.canBeIn &&
    bag.canBeIn.forEach((x) => getBagColoursBagCanBeIn(colours, x));
};

const numberOfBags = (colour: string, bags: bag[]): number => {
  const bag = bags.find((x) => x.colour === colour);
  let total = 1;

  bag.mustContain.forEach((x) => {
    if (x === null) return;
    total += x.number * numberOfBags(x.colour, bags);
  });
  return total;
};

const createGraph = (bags: bag[]): bagNode[] => {
  const bagGraph = bags.map((x) => {
    return { colour: x.colour, canBeIn: [] };
  });

  bags.map((bag) => {
    bag.mustContain.map((mustContain) => {
      if (mustContain === null) return;
      const bagInGraph = bagGraph.find(
        (node) => node.colour === mustContain.colour
      );
      bagInGraph.canBeIn.push(
        bagGraph.find((node) => node.colour === bag.colour)
      );
    });
  });
  return bagGraph;
};

interface bag {
  colour: string;
  mustContain: mustContain[];
}

interface mustContain {
  number: number;
  colour: string;
}

interface bagNode {
  colour: string;
  canBeIn: bagNode[];
}

const parse = (input: string): bag => {
  const lineRegex = input.match(/^(.*) bags contain (.*)/);
  const canContainString = lineRegex[2];
  const canContainBags = canContainString.split(",");
  const canContainColours = canContainBags.map(parseBagColour);

  return {
    colour: lineRegex[1],
    mustContain: canContainColours,
  };
};

const parseBagColour = (input: string): mustContain => {
  const regexResult = input.match(/([0-9]+) (.*) bag/);
  if (regexResult === null) {
    return null;
  }
  return { number: parseInt(regexResult[1]), colour: regexResult[2] };
};

console.log(`Part 1: ${partOne(file)}`);
console.log(`Part 2: ${partTwo(file)}`);
