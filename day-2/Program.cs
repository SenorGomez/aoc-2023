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

        List<int> possibleGameIds = PossibleGames(filePath, cubeConfiguration.CubeCounts);

        int sumOfIds = possibleGameIds.Sum();

        Console.WriteLine($"Possible games: {string.Join(", ", possibleGameIds)}");
        Console.WriteLine($"Sum of IDs: {sumOfIds}");
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

    internal class CubeConfiguration
    {
        public Dictionary<string, int> CubeCounts { get; set; }
    }
}