namespace AdventOfCode2022;

public class Day07
{
    [TestCase("data/07 - Sample.txt", 95437, TestName = "Day 07 - Part 1 - Sample")]
    [TestCase("data/07 - Puzzle Input.txt", 1182909, TestName = "Day 07 - Part 1 - Puzzle Input")]
    public void Part1(string inputFile, double expected)
    {
        var fileSystem = ParseFileSystem(File.ReadAllLines(inputFile));

        var answer = fileSystem.Directories
            .Where(x => x.Size < 100000)
            .Sum(x => x.Size);

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    [TestCase("data/07 - Sample.txt", 24933642, TestName = "Day 07 - Part 2 - Sample")]
    [TestCase("data/07 - Puzzle Input.txt", 2832508, TestName = "Day 07 - Part 2 - Puzzle Input")]
    public void Part2(string inputFile, double expected)
    {
        var fileSystem = ParseFileSystem(File.ReadAllLines(inputFile));

        var avaliableSpace = 70000000 - fileSystem.Root.Size;
        var spaceToDelete = 30000000 - avaliableSpace;

        var answer = fileSystem.Directories
            .Where(x => x.Size > spaceToDelete)
            .MinBy(x => x.Size)!
            .Size;

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    private static FileSystem ParseFileSystem(string[] input)
    {
        var fileSystem = new FileSystem();

        foreach (var line in input.Skip(1))
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
                fileSystem.FoundFile(fileInfo[1], double.Parse(fileInfo[0]));
            }
        }

        return fileSystem;
    }

    private class FileSystem
    {
        public Directory Root { get; }
        public Directory CurrentDir { get; private set; }
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

        public void FoundFile(string name, double size)
        {
            CurrentDir.Files.Add(new FileDetails(name, size));
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

    private record FileDetails(string Name, double Size);
}