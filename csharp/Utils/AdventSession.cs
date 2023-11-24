using System.Diagnostics;

namespace Utils;

public class AdventSession(int year, int day, string name = "")
{
    private string? _filename;
    private readonly Stopwatch _timer = new();

    public async Task<string> Start(string filename)
    {
        var input = await GetInput(filename);
        _filename = filename;
        _timer.Start();
        return input;
    }

    private async Task<string> GetInput(string filename)
    {
        var filePath = $"../../../Day{day:00}/{filename}";

        if (File.Exists(filePath))
        {
            return await File.ReadAllTextAsync(filePath);
        }

        if (filename.Contains("Puzzle Input"))
        {
            var sessionToken = Environment.GetEnvironmentVariable("AOC_SESSION_COOKIE") ??
                throw new ArgumentException("AOC_SESSION_COOKIE environment variable has not been set. Cannot download Puzzle Input");

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Cookie", $"session={sessionToken}");
            var input = await httpClient.GetStringAsync(new Uri($"https://adventofcode.com/{year}/day/{day}/input"));
            input = input.Trim('\r', '\n');
            await File.WriteAllTextAsync($"{filePath}", input);
            return input;
        }

        throw new FileNotFoundException("File is not the puzzle input and can not be found", filename);
    }

    public void PrintHeading() => Console.Error.WriteLine($"\n--- {year} - Day {day}: {name}");

    public void PrintAnswer(int part, object answer)
    {
        _timer.Stop();
        Console.Error.WriteLine($"Part {part,-2} {"(" + _filename + ")",-20} {answer,-15} {GetHumanTime(_timer.ElapsedMilliseconds)}");
    }

    private static string GetHumanTime(long milliseconds)
    {
        var time = TimeSpan.FromMilliseconds(milliseconds);
        return time switch
        {
            { TotalMinutes: > 1 } => $"{time.TotalMinutes:0.00} mins",
            { TotalSeconds: > 1 } => $"{time.TotalSeconds:0.00} secs",
            { TotalMilliseconds: > 1 } => $"{time.TotalMilliseconds:0} ms",
            _ => $"{time.TotalMilliseconds} ms"
        };

    }
}
