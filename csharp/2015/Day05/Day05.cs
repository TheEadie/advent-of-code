namespace AdventOfCode2015.Day05
{
    public class Day05
    {
        private readonly AdventSession _session = new(2015, 5, "Doesn't He Have Intern-Elves For This?");

        [OneTimeSetUp]
        public void SetUp()
        {
            _session.PrintHeading();
        }
        
        [Test]
        public async Task Part1()
        {
            var input = await _session.Start("Puzzle Input.txt");
            var list = input.Split("\n");

            var nice = list
                .Where(x => x.Count(l => l is 'a' or 'e' or 'i' or 'o' or 'u') >= 3)
                .Where(x => x.Zip(x.Skip(1), (curr, next) => curr == next).Any(d => d))
                .Where(x => !(x.Contains("ab") || x.Contains("cd") || x.Contains("pq") || x.Contains("xy")));


            var answer = nice.Count();

            Console.WriteLine(answer);
            answer.ShouldBe(258);
        }

        [Test]
        public async Task Part2()
        {
            var input = await _session.Start("Puzzle Input.txt");
            var list = input.Split("\n");

            var nice = list
                .Where(x => Enumerable.Range(0, x.Length - 1)
                    .Any(i => x.IndexOf(x.Substring(i, 2), i + 2, StringComparison.Ordinal) >= 0))
                .Where(x => x.Zip(x.Skip(2), (curr, next) => curr == next).Count(x => x) >= 1);


            var answer = nice.Count();

            Console.WriteLine(answer);
            answer.ShouldBe(53);
        }
    }


}
