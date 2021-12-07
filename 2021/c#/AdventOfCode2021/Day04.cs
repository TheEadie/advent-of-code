using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode2021
{
    public class Day04
    {
        [Test]
        public void Part1()
        {
            var (calls, boards) = ParseInput();
            var winningBoards = PlayBingo(calls, boards);

            var score = winningBoards.First().Item2;
            
            Console.WriteLine(score);
            score.ShouldBe(35711);
            
        }

        [Test]
        public void Part2()
        {
            var (calls, boards) = ParseInput();
            var winningBoards = PlayBingo(calls, boards);

            var score = winningBoards.Last().Item2;
            Console.WriteLine(score);
            score.ShouldBe(5586);
        }

        private static IEnumerable<(BingoBoard, int)> PlayBingo(IEnumerable<int> calls, ICollection<BingoBoard> boards)
        {
            var winningBoards = new List<(BingoBoard, int)>();

            foreach (var call in calls)
            {
                foreach (var board in boards)
                {
                    board.Mark(call);
                    if (board.HasWon())
                    {
                        winningBoards.Add((board, board.GetScore() * call));
                    }
                }

                foreach (var winningBoard in winningBoards)
                {
                    boards.Remove(winningBoard.Item1);
                }
            }

            return winningBoards;
        }

        private static (int[], List<BingoBoard>) ParseInput()
        {
            var lines = File.ReadAllLines("Day04.txt");
            var calls = lines[0].Split(',').Select(int.Parse).ToArray();

            var boardBatch = new List<string>();
            var boards = new List<BingoBoard>();
            foreach (var line in lines.Skip(2))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    boards.Add(ParseBoard(boardBatch));
                    boardBatch.Clear();
                }
                else
                {
                    boardBatch.Add(line);
                }
            }

            return (calls, boards);
        }

        private static BingoBoard ParseBoard(IEnumerable<string> input)
        {
            var rows = input.Select(line => line.Split(' ').ToList().Where(x => x != "").Select(int.Parse).ToList()).ToList();
            return new BingoBoard(rows.First().Count, rows.Count, rows);
        }
    }

    internal class BingoBoard
    {
        private readonly int[,] _board;
        private readonly bool[,] _marked;

        private readonly int _x;
        private readonly int _y;
        
        public BingoBoard(int x, int y, IEnumerable<IEnumerable<int>> values)
        {
            _board = new int[x, y];
            _marked = new bool[x, y];
            _x = x;
            _y = y;

            var i = 0;
            foreach (var line in values)
            {
                var j = 0;
                foreach (var item in line)
                {
                    _board[i, j] = item;
                    j++;
                }
                i++;
            }
        }

        public void Mark(int number)
        {
            for (var i = 0; i < _x; i++)
            {
                for (var j = 0; j < _y; j++)
                {
                    if (_board[i, j] == number)
                        _marked[i, j] = true;
                }
            }
        }

        public bool HasWon()
        {
            for (var i = 0; i < _x; i++)
            {
                var row = new List<bool>();
                for (var j = 0; j < _y; j++)
                {
                    row.Add(_marked[i, j]);
                }
                if (row.All(x => x)) return true;
            }
            
            for (var j = 0; j < _y; j++)
            {
                var col = new List<bool>();
                for (var i = 0; i < _x; i++)
                {
                    col.Add(_marked[i, j]);
                }
                if (col.All(x => x)) return true;
            }

            return false;
        }

        public int GetScore()
        {
            var score = 0;
            for (var i = 0; i < _x; i++)
            {
                for (var j = 0; j < _y; j++)
                {
                    if (!_marked[i, j])
                        score += _board[i, j];
                }
            }

            return score;
        }
    }
}