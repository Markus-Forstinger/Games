using System.Threading;

namespace BoardTest;

public static class BoardDemo
{
    /*
    public static void Run()
    {
        
        Console.WriteLine("Board testen, läuft automatisch!");
        Console.WriteLine("================================");

        Console.WriteLine("Board mit Titel initialisieren");
        Board.Init(10, 5, "Testboard", cellWidth: 60, cellHeight: 48, fontSize: 18);
        Thread.Sleep(2000);

        Console.WriteLine("Text auf Board schreiben");
        Board.SetText(3, 3, "X");
        Thread.Sleep(2000);

        string? text = Board.GetText(3, 3);
        Console.WriteLine($"Text von Board lesen, gelesener Text: {text}");
        Thread.Sleep(2000);

        Console.WriteLine("Board löschen");
        Board.Clear();
        Thread.Sleep(2000);

        Console.WriteLine("Text in Farbe rausschreiben");
        Board.SetText(0, 2, "R", "Red");
        Board.SetText(1, 2, "G", "Green");
        Board.SetText(2, 2, "S", "Black");
        Thread.Sleep(2000);

        Console.WriteLine($"Dimensionen: Zeilen={Board.GetLength(0)}, Spalten={Board.GetLength(1)}");
        Thread.Sleep(2000);

        // --- Click-Beispiel ---
        Board.Clear();
        Console.WriteLine();
        Console.WriteLine("=== Click-Beispiel ===");
        Console.WriteLine("Klicke auf eine Zelle, um sie zu markieren (erneuter Klick löscht sie).");
        Console.WriteLine("Nach 5 Klicks endet das Beispiel.");

        for (int i = 1; i <= 5; i++)
        {
            var (row, col) = Board.WaitForClick();
            if (row < 0) return; // Fenster wurde geschlossen

            string current = Board.GetText(row, col) ?? "";
            if (current == "")
                Board.SetText(row, col, "X", "Red");
            else
                Board.SetText(row, col, "");   // zweiter Klick: löschen

            Console.WriteLine($"  Klick {i}/5 → Zelle ({row}, {col})");
        }

        Console.Write("Beenden mit der Eingabetaste ...");
        Console.ReadLine();
        Board.Exit();
        
    }*/
    public static void Run()
    {
        Board.Init(9, 9, "Battleship", cellWidth: 80, cellHeight: 70, fontSize: 32);
        
        string currentPlayer = "X";
        string playerColor = "Red";
        int moves = 0;
        bool isGameActive = true;

        while (isGameActive)
        {
           var( row, col) = Board.WaitForClick();

            if (row < 0)
            {
                isGameActive = false;
            }
            else if (Board.GetText(row, col) != "")
            {
                Console.WriteLine("This cell is already occupied! Choose another one.");
            }
            else
            {
                Board.SetText(row, col, currentPlayer, playerColor);
                moves++;

                if (CheckWin(currentPlayer) || moves == 9)
                {
                    if (CheckWin(currentPlayer))
                    {
                        Console.WriteLine($"Player {currentPlayer} won!");
                    }
                    else
                    {
                        Console.WriteLine("Draw!");
                    }
                    
                    Console.WriteLine("The game is over. Click once more on the board to exit.");
                    Board.WaitForClick();
                    Board.Exit();
                    isGameActive = false;
                }
                else
                {
                    if (currentPlayer == "X")
                    {
                        currentPlayer = "O";
                        playerColor = "Green";
                    }
                    else
                    {
                        currentPlayer = "X";
                        playerColor = "Red";
                    }
                    Console.WriteLine($"Player {currentPlayer}'s turn.");
                }
            }
        }
    }

    private static bool CheckWin(string player)
    {
        for (int row = 0; row < 3; row++)
        {
            if (Board.GetText(row, 0) == player && Board.GetText(row, 1) == player && Board.GetText(row, 2) == player)
            {
                return true;
            }
        }

        for (int column = 0; column < 3; column++)
        {
            if (Board.GetText(0, column) == player && Board.GetText(1, column) == player && Board.GetText(2, column) == player)
            {
                return true;
            }
        }

        if (Board.GetText(0, 0) == player && Board.GetText(1, 1) == player && Board.GetText(2, 2) == player)
        {
            return true;
        }

        if (Board.GetText(0, 2) == player && Board.GetText(1, 1) == player && Board.GetText(2, 0) == player)
        {
            return true;
        }

        return false;
    }
}


