using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main()
    {
        // Build configuration
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        // Deserialize the configuration
        CubeConfiguration cubeConfiguration = new CubeConfiguration();
        configuration.GetSection("CubeConfiguration").Bind(cubeConfiguration);

        // Specify the path to your input file
        string filePath = "inputData.txt";

        // Find possible games
        List<int> possibleGameIds = PossibleGames(filePath, cubeConfiguration.CubeCounts);

        // Calculate the sum of IDs
        int sumOfIds = possibleGameIds.Sum();

        Console.WriteLine("Possible games: " + string.Join(", ", possibleGameIds));
        Console.WriteLine("Sum of IDs: " + sumOfIds);
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
                int count = int.Parse(parts[0]);

                if (subsetCounts.ContainsKey(color))
                {
                    subsetCounts[color] += count;
                }
                else
                {
                    subsetCounts[color] = count;
                }
            }

            foreach (var entry in subsetCounts)
            {
                string color = entry.Key;
                int count = entry.Value;

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

        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                List<string> parts = line.Split(":").ToList();
                int gameId = int.Parse(parts[0].Split(' ')[1].Trim());
                List<List<string>> subsets = parts[1].Split(';').Select(subset => subset.Trim().Split(", ").ToList()).ToList();

                if (IsPossible(subsets, cubeCounts))
                {
                    possibleGameIds.Add(gameId);
                }
            }
        }

        return possibleGameIds;
    }

    internal class CubeConfiguration
    {
        public Dictionary<string, int> CubeCounts { get; set; }
    }
}