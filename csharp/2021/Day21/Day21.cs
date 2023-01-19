namespace AdventOfCode2021.Day21
{
    public class Day21
    {
        private readonly AdventSession _session = new(2021, 21, "Dirac Dice");

        [OneTimeSetUp]
        public void SetUp()
        {
            _session.PrintHeading();
        }
        
        [Test]
        public async Task Part1()
        {
            var _ = await _session.Start("Puzzle Input.txt");
            var state = new GameState(new Player(1, 0), new Player(0, 0));
            var dice = 1;
            var player = 0;

            while (state.One.Score < 1000 && state.Two.Score < 1000)
            {
                var total = 3 * dice + 3;
                dice += 3;
                state = player == 0 ? MovePlayerOne(state, total) : MovePlayerTwo(state, total);
                player++;
                player %= 2;
            }

            var losingScore = Math.Min(state.One.Score, state.Two.Score);
            var answer = losingScore * (dice - 1);

            _session.PrintAnswer(1, answer);
            answer.ShouldBe(428736);

        }

        [Test]
        public async Task Part2()
        {
            var _ = await _session.Start("Puzzle Input.txt");
            var states = new Dictionary<GameState, long>();
            states.Add(new GameState(new Player(1, 0), new Player(0, 0)), 1);
            var player = 0;
            var totalPlayerOneWins = 0L;
            var totalPlayerTwoWins = 0L;

            while (states.Any())
            {
                states = player == 0 ? PlayerOneTurn(states) : PlayerTwoTurn(states);
                (var playerOneWins, var playerTwoWins, states) = GetWinners(states);

                totalPlayerOneWins += playerOneWins;
                totalPlayerTwoWins += playerTwoWins;

                player++;
                player %= 2;
            }

            var answer = Math.Max(totalPlayerOneWins, totalPlayerTwoWins);
            _session.PrintAnswer(2, answer);
            answer.ShouldBe(57328067654557);
        }

        private Dictionary<GameState, long> PlayerOneTurn(Dictionary<GameState,long> current)
        {
            var newStates = new Dictionary<GameState, long>();

            foreach (var (state, howMany) in current)
            {
                var move3 = MovePlayerOne(state, 3);
                var move4 = MovePlayerOne(state, 4);
                var move5 = MovePlayerOne(state, 5);
                var move6 = MovePlayerOne(state, 6);
                var move7 = MovePlayerOne(state, 7);
                var move8 = MovePlayerOne(state, 8);
                var move9 = MovePlayerOne(state, 9);

                AddState(newStates, move3, howMany);
                AddState(newStates, move4, howMany * 3);
                AddState(newStates, move5, howMany * 6);
                AddState(newStates, move6, howMany * 7);
                AddState(newStates, move7, howMany * 6);
                AddState(newStates, move8, howMany * 3);
                AddState(newStates, move9, howMany);
            }

            return newStates;
        }

        private static void AddState(Dictionary<GameState, long> newStates, GameState move, long howMany)
        {
            if (newStates.ContainsKey(move))
            {
                newStates[move] += howMany;
            }
            else
            {
                newStates.Add(move, howMany);
            }
        }

        private Dictionary<GameState, long> PlayerTwoTurn(Dictionary<GameState,long> current)
        {
            var newStates = new Dictionary<GameState, long>();

            foreach (var (state, howMany) in current)
            {
                var move3 = MovePlayerTwo(state, 3);
                var move4 = MovePlayerTwo(state, 4);
                var move5 = MovePlayerTwo(state, 5);
                var move6 = MovePlayerTwo(state, 6);
                var move7 = MovePlayerTwo(state, 7);
                var move8 = MovePlayerTwo(state, 8);
                var move9 = MovePlayerTwo(state, 9);

                AddState(newStates, move3, howMany);
                AddState(newStates, move4, howMany * 3);
                AddState(newStates, move5, howMany * 6);
                AddState(newStates, move6, howMany * 7);
                AddState(newStates, move7, howMany * 6);
                AddState(newStates, move8, howMany * 3);
                AddState(newStates, move9, howMany);
            }

            return newStates;
        }

        private (long, long, Dictionary<GameState, long>) GetWinners(Dictionary<GameState,long> current)
        {
            var playerOneWins = 0L;
            var playerTwoWins = 0L;
            var newStates = new Dictionary<GameState, long>();

            foreach (var (state, howMany) in current)
            {
                if (state.One.Score >= 21)
                {
                    playerOneWins += howMany;
                    continue;
                }
                if (state.Two.Score >= 21)
                {
                    playerTwoWins += howMany;
                    continue;
                }
                AddState(newStates, state, howMany);
            }

            return (playerOneWins, playerTwoWins, newStates);
        }

        private static GameState MovePlayerOne(GameState state, int roll)
        {
            var newPosition = state.One.Position + roll;
            newPosition %= 10;
            var newScore = state.One.Score + (newPosition == 0 ? 10 : newPosition);

            return new GameState(new Player(newPosition, newScore), state.Two);
        }

        private static GameState MovePlayerTwo(GameState state, int roll)
        {
            var newPosition = state.Two.Position + roll;
            newPosition %= 10;
            var newScore = state.Two.Score + (newPosition == 0 ? 10 : newPosition);

            return new GameState(state.One, new Player(newPosition, newScore));
        }

        private record GameState(Player One, Player Two);

        private record Player(int Position, int Score);
    }
}
