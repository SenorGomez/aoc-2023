using System;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        // Specify the path to your input file
        string filePath = "inputData.txt";

        // Read all lines from the file
        string[] lines = File.ReadAllLines(filePath);

        // Regular expression pattern to match the first and last digit on a line
        string pattern = @"(?:\d|one|two|three|four|five|six|seven|eight|nine|zero+)";

        // Variable to store the total sum
        int totalSum = 0;

        // Iterate through each line
        foreach (string line in lines)
        {
            // Use regex to match both left to right and right to left
            MatchCollection matches = Regex.Matches(line, pattern);
            MatchCollection matches2 = Regex.Matches(line, pattern, RegexOptions.RightToLeft);

            // Check if there are at least two matches
            if (matches.Count >= 1 && matches2.Count >=1)
            {
                // Extract the first and last matched values
                string firstValue = matches[0].Value;
                string lastValue = matches2[0].Value;

                // Convert the matched values to integers
                int firstDigit = GetNumericValue(firstValue);
                int lastDigit = GetNumericValue(lastValue);

                // Create a two-digit number
                int twoDigitNumber = firstDigit * 10 + lastDigit;

                // Add the two-digit number to the total sum
                totalSum += twoDigitNumber;
            }
        }

        // Display the total sum
        Console.WriteLine($"Total Sum: {totalSum}");
    }

    // Helper method to convert word representation of a digit to numeric value
    static int GetNumericValue(string wordRepresentation)
    {
        switch (wordRepresentation.ToLower())
        {
            case "one": return 1;
            case "two": return 2;
            case "three": return 3;
            case "four": return 4;
            case "five": return 5;
            case "six": return 6;
            case "seven": return 7;
            case "eight": return 8;
            case "nine": return 9;
            case "zero": return 0;
            default: return int.Parse(wordRepresentation); // Use int.Parse for numeric digits
        }
    }
}
