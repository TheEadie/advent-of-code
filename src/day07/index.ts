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
  return 0;
};

const getBagColoursBagCanBeIn = (colours: string[], bag: bagNode) => {
  colours.push(bag.colour);
  bag.canBeIn &&
    bag.canBeIn.forEach((x) => getBagColoursBagCanBeIn(colours, x));
};

const createGraph = (bags: bag[]): bagNode[] => {
  const bagGraph = bags.map((x) => {
    return { colour: x.colour, canBeIn: [] };
  });

  bags.map((bag) => {
    bag.canContain.map((bagColour) => {
      const bagInGraph = bagGraph.find((node) => node.colour === bagColour);
      if (bagInGraph === undefined) return;
      bagInGraph.canBeIn.push(
        bagGraph.find((node) => node.colour === bag.colour)
      );
    });
  });
  return bagGraph;
};

interface bag {
  colour: string;
  canContain: string[];
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
    canContain: canContainColours,
  };
};

const parseBagColour = (input: string): string => {
  const regexResult = input.match(/[0-9]+ (.*) bag/);
  if (regexResult === null) {
    return null;
  }
  return regexResult[1];
};

console.log(`Part 1: ${partOne(file)}`);
console.log(`Part 2: ${partTwo(file)}`);
