using System.Globalization;
using System.Text.RegularExpressions;

namespace MergeLogs;

public class Program
{
    static void Main(string[] args)
    {
        var directoryPath = string.Empty;

        if (args.Count() == 0 || string.IsNullOrEmpty(args[0]))
        {
            Console.WriteLine("Enter log root directory path:");

            var input = Console.ReadLine();

            while (!Directory.Exists(input))
            {
                Console.WriteLine("Directory does not exsist\n\tTry again:");
                input = Console.ReadLine();
            }

            directoryPath = input;
        }

        List<string> matchedLines = new List<string>();
        string[] logFiles = Directory.GetFiles(directoryPath, "*.log", SearchOption.AllDirectories);
        var regex = new Regex(@"^(\d{4})-(\d{2})-(\d{2}) (\d{2}):(\d{2}):(\d{2})\.(\d{3})");

        foreach (string file in logFiles)
        {
            var lines = 
                File.ReadLines(file)
                    .Where(line => regex.IsMatch(line))
                    .ToList();

            matchedLines.AddRange(lines);
        }

        matchedLines.Sort();
        matchedLines.ForEach(Console.WriteLine);
        string outputFile = Path.Combine(directoryPath, "merged.log");
        File.WriteAllLines(outputFile, matchedLines);
    }
}
