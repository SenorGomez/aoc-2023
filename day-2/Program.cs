using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        CubeConfiguration cubeConfiguration = configuration.GetSection("CubeConfiguration").Get<CubeConfiguration>();

        string filePath = "inputData.txt";

        // Part One
        List<int> possibleGameIds = PossibleGames(filePath, cubeConfiguration.CubeCounts);
        int sumOfIds = possibleGameIds.Sum();
        Console.WriteLine($"Part One - Possible games: {string.Join(", ", possibleGameIds)}");
        Console.WriteLine($"Part One - Sum of IDs: {sumOfIds}");

        // Part Two
        long sumOfPowers = SumOfPowers(filePath);
        Console.WriteLine($"Part Two - Sum of Powers: {sumOfPowers}");

    }

    static bool IsPossible(List<List<string>> game, Dictionary<string, int> cubeCounts)
    {
        foreach (var subset in game)
        {
            Dictionary<string, int> subsetCounts = new Dictionary<string, int>();

            foreach (var item in subset)
            {
                List<string> parts = item.Split().ToList();
                string color = parts[1];
                int count;

                if (int.TryParse(parts[0], out count))
                {
                    subsetCounts.TryGetValue(color, out int currentCount);
                    subsetCounts[color] = currentCount + count;
                }
                else
                {
                    Console.WriteLine($"Error parsing count for subset: {item}");
                }
            }

            foreach (var (color, count) in subsetCounts)
            {
                if (count > cubeCounts[color])
                {
                    return false;
                }
            }
        }

        return true;
    }

    static List<int> PossibleGames(string filePath, Dictionary<string, int> cubeCounts)
    {
        List<int> possibleGameIds = new List<int>();

        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    List<string> parts = line.Split(':').ToList();
                    int gameId;

                    if (int.TryParse(parts[0].Split(' ')[1].Trim(), out gameId))
                    {
                        List<List<string>> subsets = parts[1].Split(';').Select(subset => subset.Trim().Split(", ").ToList()).ToList();

                        if (IsPossible(subsets, cubeCounts))
                        {
                            possibleGameIds.Add(gameId);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error parsing game ID for line: {line}");
                    }
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }

        return possibleGameIds;
    }


    static long SumOfPowers(string filePath)
    {
        long sumOfPowers = 0;

        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    List<string> parts = line.Split(':').ToList();
                    List<List<string>> subsets = parts[1].Split(';').Select(subset => subset.Trim().Split(", ").ToList()).ToList();

                    // Initialize minimum counts for each color
                    int minRed = 0;
                    int minGreen = 0;
                    int minBlue = 0;

                    foreach (var subset in subsets)
                    {
                        Dictionary<string, int> subsetCounts = new Dictionary<string, int>();

                        foreach (var item in subset)
                        {
                            List<string> itemParts = item.Split().ToList();
                            string color = itemParts[1];
                            int count;

                            if (int.TryParse(itemParts[0], out count))
                            {
                                subsetCounts.TryGetValue(color, out int currentCount);
                                subsetCounts[color] = currentCount + count;
                            }
                        }

                        // Update minimum counts for each color
                        minRed = Math.Max(minRed, subsetCounts.GetValueOrDefault("red", 0));
                        minGreen = Math.Max(minGreen, subsetCounts.GetValueOrDefault("green", 0));
                        minBlue = Math.Max(minBlue, subsetCounts.GetValueOrDefault("blue", 0));
                    }

                    // Calculate the power for each game and sum them up
                    long power = minRed * minGreen * minBlue;
                    sumOfPowers += power;
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }

        return sumOfPowers;
    }


    internal class CubeConfiguration
    {
        public Dictionary<string, int> CubeCounts { get; set; }
    }
}