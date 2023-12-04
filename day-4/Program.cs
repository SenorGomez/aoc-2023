using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        // Specify the file path
        string filePath = "scratchCards.txt";

        // Read input from the file
        IEnumerable<string> input = File.ReadLines(filePath);

        // Calculate total points
        int totalPoints = CalculateTotalPoints(input);

        // Output result
        Console.WriteLine("Total points: " + totalPoints);
    }

    static int CalculateTotalPoints(IEnumerable<string> input)
    {
        return input.Sum(CalculateCardPoints);
    }

    static int CalculateCardPoints(string card)
    {
        int cardPoints = 0;

        // Define the regex patterns
        string winningNumbersPattern = @"Card\s+(\d+):\s*(\d+(?:\s+\d+)*)\s*\|";
        string myNumbersPattern = @"\|\s*(\d+(?:\s+\d+)*)$";

        // Use Regex.Match to find the matches
        Match winningNumbersMatch = Regex.Match(card, winningNumbersPattern);
        Match myNumbersMatch = Regex.Match(card, myNumbersPattern);

        if (winningNumbersMatch.Success && myNumbersMatch.Success)
        {
            // Extract card number and numbers after "Card"
            // Split the numbers into a hashset for lookup
            var winningNumbers = Regex.Split(winningNumbersMatch.Groups[2].Value, @"\s+").ToHashSet();
            var myNumbers = Regex.Split(myNumbersMatch.Groups[1].Value, @"\s+");

            // Calculate points for the card using HashSet for efficient lookups
            int multiplier = 1;

            foreach (var num in myNumbers)
            {
                if (winningNumbers.Contains(num))
                {
                    cardPoints = multiplier;
                    multiplier *= 2;
                }
            }
        }

        return cardPoints;
    }
}
