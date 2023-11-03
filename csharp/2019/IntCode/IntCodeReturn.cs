namespace AdventOfCode2019.IntCode;

public record IntCodeReturn(IntCodeStatus Status, IEnumerable<long> Outputs);
