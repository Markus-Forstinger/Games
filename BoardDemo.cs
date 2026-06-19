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
        Board.Init(19, 9, "Battleship", cellWidth: 34, cellHeight: 22);
        bool playersTurn = true;
        
        for(int i = 0; i< 9;i++)
        {
            Board.SetText(i,9, "-");
        }
        

        for(int i = 0; i< 5; i++)
        {
            while(playersTurn)
            {
                Console.WriteLine($"Place your {i+2}x1 ship");
                var( row, col) = Board.WaitForClick();

                if (row >= 0 && row <= 8 && col >= 0 && col <= 8)
                {
                    bool directory = false;

                    while(!directory)
                    {
                        var( row1, col1) = Board.WaitForClick();
                        
                        if(row1 == row || col1 == col)
                        {
                            directory = true;
                            
                            for(int j = 0; j< i+2; j++)
                            {
                                Board.SetText(row, col, "x");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Wrong directory");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Input");
                }
            }
        }


        
}


