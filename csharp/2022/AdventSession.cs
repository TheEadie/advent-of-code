using System.Diagnostics;

namespace AdventOfCode2022;

public class AdventSession
{
    private readonly int _year;
    private readonly int _day;
    private readonly string _name;
    private string? _filename;
    private readonly Stopwatch _timer;

    public AdventSession(int year, int day, string name = "")
    {
        _year = year;
        _day = day;
        _name = name;
        _timer = new Stopwatch();
    }

    public async Task<string> Start(string filename)
    {
        var input = await GetInput(filename);
        _filename = filename;
        _timer.Start();
        return input;
    }
    
    private async Task<string> GetInput(string filename)
    {
        var filePath = $"../../../Day{_day:00}/{filename}";
        
        if (File.Exists(filePath))
        {
            return await File.ReadAllTextAsync(filePath);
        }

        if (filename.Contains("Puzzle Input"))
        {
            var sessionToken = Environment.GetEnvironmentVariable("AOC_SESSION_COOKIE");
            if (sessionToken is null)
                throw new ArgumentException("AOC_SESSION_COOKIE environment variable has not been set. Cannot download Puzzle Input");
            
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Cookie", $"session={sessionToken}");
            var input = await httpClient.GetStringAsync(new Uri($"https://adventofcode.com/{_year}/day/{_day}/input"));
            input = input.Trim('\r', '\n');
            await File.WriteAllTextAsync($"{filePath}", input);
            return input;
        }
        
        throw new FileNotFoundException("File is not the puzzle input and can not be found", filename);
    }

    public void PrintHeading()
    {
        TestContext.Progress.WriteLine($"\n--- {_year} - Day {_day}: {_name}\n");
    }

    public void PrintAnswer(int part, object answer)
    {
        _timer.Stop();
        TestContext.Progress.WriteLine($"Part {part}: ({_filename}) {answer} - {_timer.ElapsedMilliseconds}ms");
    }
}