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
        //Hangman
        if (choice == 3)
        {
            global::BoardTest.Hangman.Run();
            continue; 
        }
        
        //Battleship
        else if (choice == 2)
        {
            string battleshipDll = Path.Combine("Battleship", "bin", "Debug", "net9.0", "Battleship.dll");
            
            //schaut ob das exe existiert sonst erstellt es
            if (File.Exists(battleshipDll))
            {
                Console.WriteLine("Starte Battleship...");
                //externes Programm starten
                var battleshipProcess = Process.Start(new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = $"\"{battleshipDll}\"",
                    //direkt starten statt windows explorer
                    UseShellExecute = false,
                    //eigenes Konsolen Fenster
                    CreateNoWindow = false
                });
                
                battleshipProcess?.WaitForExit();
            }
            else
            {
                Console.WriteLine("Battleship.dll wurde nicht gefunden. Bitte erst bauen!");
                Console.ReadKey();
            }
            continue;
        }

        //TicTacToe

        //existiert die exe, wenn nicht mach sie    
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
            
            //Prozess ned gestartet?
            if (build != null)
            {
                //wartet auf build
                build.WaitForExit();
                //Build gegangen?
                if (build.ExitCode != 0)
                {
                    //Fehlermeldung
                    string error = build.StandardError.ReadToEnd();
                    Console.WriteLine($"Build failed: {error}");
                    Console.ReadKey();
                    continue;
                }
            }
        }
        //exe starten
        if (File.Exists(exePath))
        {
            Process.Start(new ProcessStartInfo 
            { 
                //exe direkt starten
                FileName = exePath, 
                UseShellExecute = false,
                //avalonia öffnet
                CreateNoWindow = true 
            });
        }
        else
        {
            //exe wurde ned gefunden
            Console.WriteLine("EXE not found after build.");
            Console.ReadKey();
        }
    }
    catch (System.Exception ex)
    {
        //falls irgenein error kommt während Laufzeit
        Console.WriteLine($"Error: {ex.Message}");
        Console.ReadKey();
    }
}