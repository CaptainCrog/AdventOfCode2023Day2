using System.Linq;
using System.Text.RegularExpressions;
// See https://aka.ms/new-console-template for more information
var filePath = @"AoCDay2Input.txt";
var continueApplication = true;

int maxRedCubes = 12;
int maxGreenCubes = 13;
int maxBlueCubes = 14;

var idPattern = @"Game (\d+):";
var roundsPattern = @"Game \d+:\s*|\s*;\s*";
var colourPattern = @",\s*";

List<Game> games = new List<Game>();

while (continueApplication)
{
    Console.WriteLine("PART 1 (1), PART 2 (2), or Quit (3)");
    var invalidInput = int.TryParse(Console.ReadLine(), out int input);

    if (!invalidInput)
    {
        Console.WriteLine("Bad Attempt: Invalid character");
    }
    else if (input == 1 || input == 2)
    {
        int answer = 0;
        foreach (var line in File.ReadAllLines(filePath))
        {
            var game = GetGameInfo(line);
            if (input == 1)
            {
                if (!game.Rounds.Any(x => x.RoundRedCubes > maxRedCubes || x.RoundGreenCubes > maxGreenCubes || x.RoundBlueCubes > maxBlueCubes))
                {
                    answer += game.Id;
                }
            }
            else
            {
                var highestRedNum = game.Rounds.Max(x => x.RoundRedCubes);
                var highestBlueNum = game.Rounds.Max(x => x.RoundBlueCubes);
                var highestGreenNum = game.Rounds.Max(x => x.RoundGreenCubes);

                var power = highestRedNum * highestBlueNum * highestGreenNum;
                answer += power;
            }
        }

        Console.WriteLine($"\nThe Answer is: {answer}\n\n");
    }
    else if (input == 3)
    {
        continueApplication = false;
    }
    else
    {
        Console.WriteLine("Bad Attempt: Number is not recognised input");
    }
}
Console.WriteLine("Exiting Application");
Environment.Exit(0);

Game GetGameInfo(string line)
{
    var game = new Game()
    {
        Id = int.Parse(Regex.Match(line, idPattern).Groups[1].Value),
    };

    var rounds = Regex.Split(line, roundsPattern);
    foreach (var round in rounds)
    {
        int redNumber = 0;
        int greenNumber = 0;
        int blueNumber = 0;


        var colours = Regex.Split(round, colourPattern);
        var redText = colours.Where(x => x.Contains("red")).FirstOrDefault();
        if (!string.IsNullOrEmpty(redText))
            redNumber = ParseColourText(redText);
        var blueText = colours.Where(x => x.Contains("blue")).FirstOrDefault();
        if (!string.IsNullOrEmpty(blueText))
            blueNumber = ParseColourText(blueText);
        var greenText = colours.Where(x => x.Contains("green")).FirstOrDefault();
        if (!string.IsNullOrEmpty(greenText))
            greenNumber = ParseColourText(greenText);

        game.Rounds.Add(new Round()
        {
            RoundRedCubes = redNumber,
            RoundGreenCubes = greenNumber,
            RoundBlueCubes = blueNumber,
        });
    }

    return game;
}

int ParseColourText(string? colourText)
{
    return int.Parse(new string(colourText.SkipWhile(c => !char.IsDigit(c))
                 .TakeWhile(c => char.IsDigit(c))
                 .ToArray()));
}

public class Game
{
    public Game()
    {
        Rounds = new List<Round>();
    }

    public int Id { get; set; }

    public List<Round> Rounds { get; set; }

}

public class Round
{
    public Round()
    {

    }

    public int RoundRedCubes { get; set; }

    public int RoundGreenCubes { get; set; }

    public int RoundBlueCubes { get; set; }
}
