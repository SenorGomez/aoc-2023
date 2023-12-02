using System;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        // Specify the path to your input file
        string filePath = "inputData.txt";


        // Part 1
        // Regular expression pattern to match the first and last digit on a line
        string patternPart1 = @"\d";

        // Variable to store the total sum
        int totalSum = GetSum(filePath, patternPart1);

        // Display the total sum
        Console.WriteLine($"Total Sum: {totalSum}");

        // Part 2
        // Regular expression pattern to match the first and last digit on a line
        string patternPart2 = @"(?:\d|one|two|three|four|five|six|seven|eight|nine|zero+)";

        // Variable to store the total sum
        totalSum = GetSum(filePath, patternPart2);

        // Display the total sum
        Console.WriteLine($"Total Sum: {totalSum}");
    }


    static int GetSum(string filePath, string pattern) {
        int totalSum = 0;

        // Open a stream reader for the file
        using (StreamReader reader = new StreamReader(filePath))
        {
            // Read each line from the file
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                if (!string.IsNullOrWhiteSpace(line))
                {
                    // Use regex to match both left to right and right to left
                    MatchCollection matches = Regex.Matches(line, pattern);
                    MatchCollection reverseMatches = Regex.Matches(line, pattern, RegexOptions.RightToLeft);

                    // Check if there are at least two matches in either direction
                    if (matches.Count >= 1 && reverseMatches.Count >= 1)
                    {
                        // Extract the first and last matched values
                        string firstValue = matches[0].Value;
                        string lastValue = reverseMatches[0].Value;

                        // Convert the matched values to integers
                        int firstDigit = GetNumericValue(firstValue);
                        int lastDigit = GetNumericValue(lastValue);

                        // Check if the numeric values are within the valid range
                        if (firstDigit >= 0 && firstDigit <= 9 && lastDigit >= 0 && lastDigit <= 9)
                        {
                            // Create a two-digit number
                            int twoDigitNumber = firstDigit * 10 + lastDigit;

                            // Add the two-digit number to the total sum
                            totalSum += twoDigitNumber;
                        }
                        else
                        {
                            Console.WriteLine($"Invalid numeric values for line: {line}");
                        }
                    }
                }
            }
        }

        return totalSum;
    }

// Helper method to convert word representation of a digit to numeric value
static int GetNumericValue(string wordRepresentation)
    {
        if (int.TryParse(wordRepresentation, out int numericValue))
        {
            return numericValue;
        }

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
            default: return -1;
        }
    }
}
