using System.Diagnostics;

while (true)
{
    Console.Clear();

    Console.WriteLine(@"
╔══════════════════════════════════╗
║        Welcome Game Suite        ║
╚══════════════════════════════════╝");

    Console.WriteLine();
    Console.WriteLine("1. Tic Tac Toe");
    Console.WriteLine("2. Battleship");
    Console.WriteLine("3. Hangman");
    Console.WriteLine("0. Exit");
    Console.WriteLine();

    Console.Write("Choice: ");

    if (!int.TryParse(Console.ReadLine(), out int choice))
    {
        Console.WriteLine("Invalid Input!");
        Console.ReadKey();
        continue;
    }

    string? game = choice switch
    {
        1 => "TicTacToe",
        2 => "Battleship",
        3 => "Hangman",
        0 => null,
        _ => ""
    };

    if (choice == 0)
        break;

    if (game == "")
    {
        Console.WriteLine("Choose a valid game!");
        Console.ReadKey();
        continue;
    }

    try
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = "run",
            WorkingDirectory = $"game/{game}",
            UseShellExecute = true
        });
    }
    catch
    {
        Console.WriteLine("Game not found!");
        Console.ReadKey();
    }
}   