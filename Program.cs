using System.Diagnostics;

Console.WriteLine(@"
  ╔══════════════════════════════════╗
  ║   Welcome to  the Game Suite     ║
  ╚══════════════════════════════════╝");

Console.WriteLine("Which game to you want to play?");
int choice;

Console.WriteLine(@"
Which game to you want to play?

1. Tic Tac Toe
2. Battleship
3. Hangman");

choice = Convert.ToInt32(Console.ReadLine());
string game = "";

switch (choice)
{
    case 1:
        game = "TicTacToe";
        break;

    case 2:
        game = "Battleship";
        break;
    
    case 3:
        game = "Hangman";
        break;
    
    default:
        Console.WriteLine("Choose a Game in the list.");
        break;
}

if (game != "")
{
    ProcessStartInfo info = new ProcessStartInfo
    {
        FileName = "dotnet",
        Arguments = "run",
        WorkingDirectory = $"game/{game}",
        UseShellExecute = true
    };

    Process.Start(info);
}
