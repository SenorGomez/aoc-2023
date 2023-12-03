using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

class Program
{
    static void Main()
    {
        var input = File.ReadAllLines("engineSchematic.txt");

        var numbers = new List<Number>();
        var symbols = new List<Symbol>();

        for (var row = 0; row < input.Length; row++)
        {
            var currentNumber = new Number();
            var digits = new List<int>();

            for (var col = 0; col < input[row].Length; col++)
            {
                if (input[row][col] == '.') { continue; }

                if (int.TryParse(input[row][col].ToString(), out var digit))
                {
                    digits.Add(digit);
                    if (digits.Count == 1)
                    {
                        currentNumber.Start = (row, col);
                    }

                    while (col < input[row].Length - 1 && int.TryParse(input[row][col + 1].ToString(), out digit))
                    {
                        digits.Add(digit);
                        col++;
                    }

                    currentNumber.End = (row, col);
                    currentNumber.Value = int.Parse(string.Join("", digits));
                    numbers.Add(currentNumber);
                    currentNumber = new Number();
                    digits.Clear();
                }
                else
                {
                    symbols.Add(new Symbol
                    {
                        Value = input[row][col],
                        Position = (row, col)
                    });
                }
            }
        }

        var part1 = numbers
            .Where(number => symbols.Any(symbol => AreAdjacent(number, symbol)))
            .Sum(number => number.Value);

        var part2 = symbols
            .Where(symbol => symbol.Value == '*')
            .Select(symbol => numbers.Where(number => AreAdjacent(number, symbol)).ToArray())
            .Where(gears => gears.Length == 2)
            .Sum(gears => gears[0].Value * gears[1].Value);

        Console.WriteLine($"Part 1: {part1}");
        Console.WriteLine($"Part 2: {part2}");
    }

    static bool AreAdjacent(Number number, Symbol symbol)
    {
        return Math.Abs(symbol.Position.Row - number.Start.Row) <= 1
               && symbol.Position.Column >= number.Start.Column - 1
               && symbol.Position.Column <= number.End.Column + 1;
    }

    internal struct Number
    {
        public int Value { get; set; }
        public (int Row, int Column) Start { get; set; }
        public (int Row, int Column) End { get; set; }
    }

    internal struct Symbol
    {
        public char Value { get; set; }
        public (int Row, int Column) Position { get; set; }

    }
}
   