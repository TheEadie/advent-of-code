namespace AdventOfCode2021.Day22
{
    public class Day22
    {
        private readonly AdventSession _session = new(2021, 22, "Reactor Reboot");

        [OneTimeSetUp]
        public void SetUp()
        {
            _session.PrintHeading();
        }
        
        [Test]
        public async Task Part1()
        {
            var input = await _session.Start("Puzzle Input.txt");
            var commands = ParseInput(input);

            commands.RemoveAll(x => x.Cube.End.X > 50 || x.Cube.Start.X < -50
                                                      || x.Cube.End.Y > 50 || x.Cube.Start.Y < -50 ||
                                                      x.Cube.End.Z > 50 || x.Cube.Start.Z < -50);

            var answer = HowManyCubesOn(commands);
            _session.PrintAnswer(1, answer);
            answer.ShouldBe(615700);

        }

        [Test]
        public async Task Part2()
        {
            var input = await _session.Start("Puzzle Input.txt");
            var commands = ParseInput(input);

            var answer = HowManyCubesOn(commands);
            _session.PrintAnswer(2, answer);
            answer.ShouldBe(1236463892941356);
        }

        private long HowManyCubesOn(List<Command> commands)
        {
            var allCubes = new List<Command>();
            foreach (var command in commands)
            {
                var intersections = new List<Command>();

                foreach (var previousCommand in allCubes)
                {
                    if (GetIntersectionVolume(command.Cube, previousCommand.Cube) > 0)
                    {
                        intersections.Add(new Command(GetIntersection(command.Cube, previousCommand.Cube),
                            !previousCommand.On));
                    }
                }

                if (command.On)
                    allCubes.Add(command);
                allCubes.AddRange(intersections);
            }

            var answer = allCubes.Sum(x => x.On ? GetVolume(x.Cube) : -GetVolume(x.Cube));
            return answer;
        }

        private List<Command> ParseInput(string input)
        {
            return input.Split("\n").Select(ParseCommand).ToList();
        }

        private Command ParseCommand(string input)
        {
            var on = input[1] == 'n';
            var xString = input.Split(' ')[1].Split(',')[0];
            var xFrom = int.Parse(xString.Split("..")[0].Split('=')[1]);
            var xTo = int.Parse(xString.Split("..")[1]);

            var yString = input.Split(',')[1];
            var yFrom = int.Parse(yString.Split("..")[0].Split('=')[1]);
            var yTo = int.Parse(yString.Split("..")[1]);

            var zString = input.Split(',')[2];
            var zFrom = int.Parse(zString.Split("..")[0].Split('=')[1]);
            var zTo = int.Parse(zString.Split("..")[1]);

            return new Command(new Cube(new Coordinate3D(xFrom, yFrom, zFrom), new Coordinate3D(xTo, yTo, zTo)), on);
        }


        private long GetIntersectionVolume(Cube a, Cube b)
        {
            var cubes = new List<Cube> {a, b};
            return Math.Max(cubes.Min(x => x.End.X) + 1L - cubes.Max(x => x.Start.X), 0) *
                   Math.Max(cubes.Min(x => x.End.Y) + 1L - cubes.Max(x => x.Start.Y), 0) *
                   Math.Max(cubes.Min(x => x.End.Z) + 1L - cubes.Max(x => x.Start.Z), 0);
        }

        private Cube GetIntersection(Cube a, Cube b)
        {
            var minX = Math.Max(a.Start.X, b.Start.X);
            var maxX = Math.Min(a.End.X, b.End.X);
            var minY = Math.Max(a.Start.Y, b.Start.Y);
            var maxY = Math.Min(a.End.Y, b.End.Y);
            var minZ = Math.Max(a.Start.Z, b.Start.Z);
            var maxZ = Math.Min(a.End.Z, b.End.Z);

            return new Cube(new Coordinate3D(minX, minY, minZ), new Coordinate3D(maxX, maxY, maxZ));
        }

        private long GetVolume(Cube cube)
        {
            return (cube.End.X + 1L - cube.Start.X) * (cube.End.Y + 1L - cube.Start.Y) * (cube.End.Z + 1L - cube.Start.Z);
        }

        private record Command(Cube Cube, bool On);

        private record Cube(Coordinate3D Start, Coordinate3D End);

        private record Coordinate3D(int X, int Y, int Z);
    }


}
