namespace AdventOfCode2021
{
    public class Day08
    {
        [Test]
        public void Part1()
        {
            var answer = ParseInput()
                .SelectMany(x => x.Result)
                .Count(x => x.Length is 2 or 3 or 4 or 7);

            Console.WriteLine(answer);
            answer.ShouldBe(397);
        }

        [Test]
        public void Part2()
        {
            var answer = ParseInput().Sum(GetSignal);

            Console.WriteLine(answer);
            answer.ShouldBe(1027422);

        }

        private static int GetSignal(Note note)
        {
            var observed = note.Observed.ToList();

            var one = observed.Single(x => x.Length == 2);
            var seven = observed.Single(x => x.Length == 3);
            var four = observed.Single(x => x.Length == 4);
            var eight = observed.Single(x => x.Length == 7);

            var fourArm = four.Except(one);

            var three = observed.Single(x => x.Length == 5 && one.All(x.Contains));
            var five = observed.Single(x => x.Length == 5 && fourArm.All(x.Contains));
            var two = observed.Except(new List<string> {three, five}).Single(x => x.Length == 5);

            var nine = observed.Single(x => x.Length == 6 && four.All(x.Contains));
            var six = observed.Except(new List<string> {nine}).Single(x => x.Length == 6 && fourArm.All(x.Contains));
            var zero = observed.Except(new List<string> {six, nine}).Single(x => x.Length == 6);

            var solved = new Dictionary<string, string>
            {
                {one, "1"},
                {two, "2"},
                {three, "3"},
                {four, "4"},
                {five, "5"},
                {six, "6"},
                {seven, "7"},
                {eight, "8"},
                {nine, "9"},
                {zero, "0"}
            };

            var signalText = string.Join("", note.Result.Select(x => solved[x]));

            return int.Parse(signalText);
        }

        private static IEnumerable<Note> ParseInput()
        {
            var lines = File.ReadAllLines("Day08.txt");
            return lines.Select(x => x.Split(" | ")).Select(x =>
                new Note(x[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(SortString),
                    x[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(SortString))).ToList();
        }

        private record Note(IEnumerable<string> Observed, IEnumerable<string> Result);

        private static string SortString(string input)
        {
            var characters = input.ToArray();
            Array.Sort(characters);
            return new string(characters);
        }
    }
}
