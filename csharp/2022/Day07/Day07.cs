namespace AdventOfCode2022.Day07;

public class Day07
{
    private readonly AdventSession _session = new(2022, 7);

    [OneTimeSetUp]
    public void SetUp()
    {
        _session.PrintHeading();
    }
    
    [TestCase("Sample.txt", 95437)]
    [TestCase("Puzzle Input.txt", 1182909)]
    public async Task Part1(string inputFile, double expected)
    {
        var input = await _session.Start(inputFile);
        var fileSystem = ParseFileSystem(input);

        var answer = fileSystem.Directories
            .Where(x => x.Size < 100000)
            .Sum(x => x.Size);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 24933642)]
    [TestCase("Puzzle Input.txt", 2832508)]
    public async Task Part2(string inputFile, double expected)
    {
        var input = await _session.Start(inputFile);
        var fileSystem = ParseFileSystem(input);

        var availableSpace = 70000000 - fileSystem.Root.Size;
        var spaceToDelete = 30000000 - availableSpace;

        var answer = fileSystem.Directories
            .Where(x => x.Size > spaceToDelete)
            .MinBy(x => x.Size)!
            .Size;

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static FileSystem ParseFileSystem(string input)
    {
        var fileSystem = new FileSystem();

        foreach (var line in input.Split("\n").Skip(1))
        {
            if (line == "$ cd ..")
            {
                fileSystem.MoveUpOne();
            }
            else if (line.StartsWith("$ cd"))
            {
                fileSystem.ChangeDir(line.Split(" ").Last());
            }
            else if (line.StartsWith("$ ls"))
            {
                // Nothing
            }
            else if (line.StartsWith("dir"))
            {
                fileSystem.FoundDir(line.Split(" ").Last());
            }
            else
            {
                var fileInfo = line.Split(" ");
                fileSystem.FoundFile(double.Parse(fileInfo[0]));
            }
        }

        return fileSystem;
    }

    private class FileSystem
    {
        public Directory Root { get; }
        private Directory CurrentDir { get; set; }
        public IList<Directory> Directories { get; }

        public FileSystem()
        {
            var root = Directory.Create(@"\", null!);
            CurrentDir = root;
            Root = root;
            Directories = new List<Directory>();
        }

        public void ChangeDir(string name)
        {
            CurrentDir = CurrentDir.Directories.Single(x => x.Name == name);
        }

        public void MoveUpOne()
        {
            CurrentDir = CurrentDir.Parent;
        }

        public void FoundDir(string name)
        {
            var dir = Directory.Create(name, CurrentDir);
            CurrentDir.Directories.Add(dir);
            Directories.Add(dir);
        }

        public void FoundFile(double size)
        {
            CurrentDir.Files.Add(new FileDetails(size));
        }
    }

    private record Directory(string Name, Directory Parent, IList<Directory> Directories, IList<FileDetails> Files)
    {
        public double Size => Directories.Sum(x => x.Size) + Files.Sum(x => x.Size);

        public static Directory Create(string name, Directory parent)
        {
            return new Directory(name, parent, new List<Directory>(), new List<FileDetails>());
        }
    }

    private record FileDetails(double Size);
}