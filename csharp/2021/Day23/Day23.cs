namespace AdventOfCode2021.Day23;

public class Day23
{
    private readonly AdventSession _session = new(2021, 23, "Amphipod");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        _ = await _session.Start("Puzzle Input.txt");
        var start = new GameState("...........", "DC", "AA", "DB", "CB");
        var goal = new GameState("...........", "AA", "BB", "CC", "DD");


        var (answer, _) = Run(start, goal);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(14546);
    }

    [Test]
    public async Task Part1Sample()
    {
        var _ = await _session.Start("Sample.txt");
        var start = new GameState("...........", "BA", "CD", "BC", "DA");
        var goal = new GameState("...........", "AA", "BB", "CC", "DD");

        var (answer, _) = Run(start, goal);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(12521);
    }

    [Test]
    public async Task Part2()
    {
        _ = await _session.Start("Puzzle Input.txt");
        var start = new GameState("...........", "DDDC", "ACBA", "DBAB", "CACB");
        var goal = new GameState("...........", "AAAA", "BBBB", "CCCC", "DDDD");

        var (answer, _) = Run(start, goal);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(42308);
    }

    [Test]
    public async Task Part2Sample()
    {
        var _ = await _session.Start("Sample.txt");
        var start = new GameState("...........", "BDDA", "CCBD", "BBAC", "DACA");
        var goal = new GameState("...........", "AAAA", "BBBB", "CCCC", "DDDD");

        var (answer, _) = Run(start, goal);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(44169);
    }

    private (int, List<GameState>) Run(GameState start, GameState goal)
    {
        var queue = new PriorityQueue<GameState, int>();

        var cameFrom = new Dictionary<GameState, GameState>();
        var costSoFar = new Dictionary<GameState, int>
        {
            [start] = 0
        };

        var totalPath = new List<GameState> { goal };

        queue.Enqueue(start, 0);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (current == goal)
            {
                while (cameFrom.ContainsKey(current))
                {
                    current = cameFrom[current];
                    totalPath = totalPath.Prepend(current).ToList();
                }

                break;
            }

            foreach (var neighbour in GetLegalMoves(current))
            {
                var tentativeGScore = costSoFar[current] + neighbour.Cost;
                if (!costSoFar.ContainsKey(neighbour.State) || tentativeGScore < costSoFar[neighbour.State])
                {
                    cameFrom[neighbour.State] = current;
                    costSoFar[neighbour.State] = tentativeGScore;
                    queue.Enqueue(neighbour.State, tentativeGScore + 0);
                }
            }
        }

        var answer = costSoFar[goal];
        return (answer, totalPath);
    }

    private IEnumerable<Move> GetLegalMoves(GameState currentState)
    {
        var moves = new List<Move>();
        moves.AddRange(GetDirectHomeMoves(currentState));
        moves.AddRange(GetMovesToHallway(currentState));
        moves.AddRange(GetMovesHome(currentState));

        return moves;
    }

    private IEnumerable<Move> GetDirectHomeMoves(GameState currentState)
    {
        // for each side room
        var sideRooms = new[] { 'A', 'B', 'C', 'D' };
        foreach (var room in sideRooms)
        {
            // if something inside get top
            var amphipod = GetTopOfSideRoom(room, currentState);
            if (amphipod == null)
            {
                continue;
            }

            // if not home
            var home = amphipod.Type;
            if (home == room && currentState.AreAllPresentHome(home))
            {
                continue;
            }

            // does home have a free slot
            if (GetNumberOfAmphipodsInSideRoom(home, currentState) == GetSideRoomSize(currentState, home)
                || currentState.GetSideRoom(home).Any(x => x != home && x != '.'))
            {
                continue;
            }

            // if so can we get there
            var sideRoomExit = GetSideRoomEntrance(room);
            var homeEntrance = GetSideRoomEntrance(home);
            if (!ClearPath(sideRoomExit, homeEntrance, currentState))
            {
                continue;
            }

            // count cost
            var distanceOut = GetSideRoomSize(currentState, room) -
                GetNumberOfAmphipodsInSideRoom(room, currentState) + 1;
            var distanceIn = GetSideRoomSize(currentState, home) -
                GetNumberOfAmphipodsInSideRoom(home, currentState);
            var distanceHallway = GetDistance(sideRoomExit, homeEntrance);

            var cost = (distanceOut + distanceIn + distanceHallway) * GetCost(amphipod.Type);
            var state = currentState.WithAmphipodRemoved(room);
            state = state.WithAmphipodAdded(amphipod.Type, home);
            yield return new Move(state, cost);
        }

    }

    private IEnumerable<Move> GetMovesToHallway(GameState currentState)
    {
        // for each side room
        var sideRooms = new[] { 'A', 'B', 'C', 'D' };
        foreach (var room in sideRooms)
        {
            // if something inside get top
            var amphipod = GetTopOfSideRoom(room, currentState);
            if (amphipod == null)
            {
                continue;
            }

            // if not home
            var home = amphipod.Type;
            if (home == room && currentState.AreAllPresentHome(home))
            {
                continue;
            }

            var validHallwayLocations = new[] { 0, 1, 3, 5, 7, 9, 10 };

            foreach (var hallwayLocation in validHallwayLocations)
            {
                // if so can we get there
                if (currentState.Hallway[hallwayLocation] != '.')
                {
                    continue;
                }

                var sideRoomExit = GetSideRoomEntrance(room);
                if (!ClearPath(sideRoomExit, hallwayLocation, currentState))
                {
                    continue;
                }

                // count cost
                var distanceOut = GetSideRoomSize(currentState, room) -
                    GetNumberOfAmphipodsInSideRoom(room, currentState) + 1;
                var distanceHallway = GetDistance(sideRoomExit, hallwayLocation);

                var cost = (distanceOut + distanceHallway) * GetCost(amphipod.Type);
                var state = currentState.WithAmphipodRemoved(room);
                state = state.WithAmphipodAddedToHallway(amphipod.Type, hallwayLocation);
                yield return new Move(state, cost);
            }
        }
    }

    private IEnumerable<Move> GetMovesHome(GameState currentState)
    {
        // for each hallway location
        var validHallwayLocations = new[] { 0, 1, 3, 5, 7, 9, 10 };

        foreach (var hallwayLocation in validHallwayLocations)
        {
            // Get in hallway
            var amphipod = currentState.Hallway[hallwayLocation];
            if (amphipod == '.')
            {
                continue;
            }

            // does home have a free slot
            var home = amphipod;
            if (GetNumberOfAmphipodsInSideRoom(home, currentState) == GetSideRoomSize(currentState, home)
                || currentState.GetSideRoom(home).Any(x => x != home && x != '.'))
            {
                continue;
            }

            // if so can we get there
            var sideRoomExit = GetSideRoomEntrance(home);
            if (!ClearPath(hallwayLocation, sideRoomExit, currentState))
            {
                continue;
            }

            // count cost
            var distanceIn = GetSideRoomSize(currentState, home) -
                             GetNumberOfAmphipodsInSideRoom(home, currentState);
            var distanceHallway = GetDistance(sideRoomExit, hallwayLocation);

            var cost = (distanceIn + distanceHallway) * GetCost(amphipod);
            var state = currentState.WithAmphipodAddedToHallway('.', hallwayLocation);
            state = state.WithAmphipodAdded(amphipod, home);
            yield return new Move(state, cost);
        }
    }



    private static int GetSideRoomSize(GameState currentState, char room) => currentState.GetSideRoom(room).Length;

    private static int GetCost(char amphipod)
    {
        return amphipod switch
        {
            'A' => 1,
            'B' => 10,
            'C' => 100,
            'D' => 1000,
            _ => throw new ArgumentException("Unknown Char")
        };
    }

    private bool ClearPath(int sideRoomExit, int homeEntrance, GameState state)
    {
        var start = Math.Min(sideRoomExit, homeEntrance);
        var end = Math.Max(sideRoomExit, homeEntrance);
        for (var i = start + 1; i < end; i++)
        {
            if (state.Hallway[i] != '.')
            {
                return false;
            }
        }

        return true;
    }

    private int GetDistance(int startPoint, int endPoint)
    {
        var start = Math.Min(startPoint, endPoint);
        var end = Math.Max(startPoint, endPoint);

        return end - start;
    }

    private int GetSideRoomEntrance(char room) => (room - 'A') * 2 + 2;

    private Amphipod? GetTopOfSideRoom(char room, GameState state)
    {
        return state.GetSideRoom(room)
            .Select(x => new Amphipod(x))
            .FirstOrDefault(x => x.Type != '.');
    }

    private int GetNumberOfAmphipodsInSideRoom(char room, GameState state) => state.GetSideRoom(room).Count(x => x != '.');

    private record Amphipod(char Type);

    private record GameState(string Hallway, string A, string B, string C, string D)
    {
        public override string ToString() => $"{Hallway} {A} {B} {C} {D}";

        public string GetSideRoom(char room)
        {
            return room switch
            {
                'A' => A,
                'B' => B,
                'C' => C,
                'D' => D,
                _ => throw new ArgumentException($"Unknown side room: {room}")
            };
        }

        public bool AreAllPresentHome(char room) => GetSideRoom(room).All(x => x == '.' || x == room);

        public GameState WithAmphipodRemoved(char room)
        {
            return room switch
            {
                'A' => new GameState(Hallway, RemoveTopOfSideRoom(A), B, C, D),
                'B' => new GameState(Hallway, A, RemoveTopOfSideRoom(B), C, D),
                'C' => new GameState(Hallway, A, B, RemoveTopOfSideRoom(C), D),
                'D' => new GameState(Hallway, A, B, C, RemoveTopOfSideRoom(D)),
                _ => throw new ArgumentException($"Unknown side room: {room}")
            };
        }

        public GameState WithAmphipodAdded(char amphipod, char room)
        {
            return room switch
            {
                'A' => new GameState(Hallway, AddToTopOfSideRoom(amphipod, A), B, C, D),
                'B' => new GameState(Hallway, A, AddToTopOfSideRoom(amphipod, B), C, D),
                'C' => new GameState(Hallway, A, B, AddToTopOfSideRoom(amphipod, C), D),
                'D' => new GameState(Hallway, A, B, C, AddToTopOfSideRoom(amphipod, D)),
                _ => throw new ArgumentException($"Unknown side room: {room}")
            };
        }

        public GameState WithAmphipodAddedToHallway(char amphipod, int location)
        {
            return new GameState(
                Hallway.Remove(location, 1).Insert(location, amphipod.ToString()),
                A, B, C, D);
        }

        private static string RemoveTopOfSideRoom(string sideRoom)
        {
            var depth = sideRoom.LastIndexOf('.') + 1;
            return sideRoom.Remove(depth, 1).Insert(depth, ".");
        }

        private static string AddToTopOfSideRoom(char amphipod, string sideRoom)
        {
            var depth = sideRoom.LastIndexOf('.');
            return sideRoom.Remove(depth, 1).Insert(depth, amphipod.ToString());
        }
    }

    private record Move(GameState State, int Cost);
}
