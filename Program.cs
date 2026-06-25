using System;
using System.Diagnostics;
using System.IO;

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

    if (choice == 0)
        break;

    string? gameFolder = choice switch
    {
        1 => "TicTacToe",
        2 => "Battleship",
        3 => "Hangman",
        _ => ""
    };

    if (gameFolder == "")
    {
        Console.WriteLine("Choose a valid game!");
        Console.ReadKey();
        continue;
    }

    string csprojFile = choice == 1 ? "BoardTest-avalonia.csproj" : $"{gameFolder}.csproj";
    string fullPath = Path.Combine(gameFolder, csprojFile);
    
    string exeName = choice == 1 ? "BoardTest-avalonia" : gameFolder;
    string exePath = Path.Combine(gameFolder, "bin", "Debug", "net9.0", $"{exeName}.exe");

    try
    {
        if (choice == 3)
        {
            global::BoardTest.Hangman.Run();
            continue; 
        }
        else if (choice == 2)
        {
            BoardTest.BoardDemo.Run();
            continue;
        }
        if (!File.Exists(exePath))
        {
            Console.WriteLine($"Building {gameFolder}...");
            var build = Process.Start(new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"build \"{fullPath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            });
            
            if (build != null)
            {
                build.WaitForExit();
                if (build.ExitCode != 0)
                {
                    string error = build.StandardError.ReadToEnd();
                    Console.WriteLine($"Build failed: {error}");
                    Console.ReadKey();
                    continue;
                }
            }
        }

        if (File.Exists(exePath))
        {
            Process.Start(new ProcessStartInfo 
            { 
                FileName = exePath, 
                UseShellExecute = false,
                CreateNoWindow = true
            });
        }
        else
        {
            Console.WriteLine("EXE not found after build.");
            Console.ReadKey();
        }
    }
    catch (System.Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        Console.ReadKey();
    }
}