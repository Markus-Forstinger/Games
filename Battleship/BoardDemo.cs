using System;
using System.Threading;

namespace BoardTest;

public static class BoardDemo
{
    public static void Run()
    {
        // 1. Spielfeldgröße auf 10 setzen (0 bis 9)
        const int SIZE = 10; 
        bool[,] ships = new bool[SIZE, SIZE];
        Random random = new();

        // 2. Deine gewünschten Schiffe (2x1, 3x1, 4x1, 5x1)
        int[] shipSizes = { 2, 3, 4, 5 };

        foreach (int size in shipSizes)
        {
            bool placed = false;
            while (!placed)
            {
                bool horizontal = random.Next(2) == 0;
                int row = random.Next(SIZE - (horizontal ? 0 : size));
                int col = random.Next(SIZE - (horizontal ? size : 0));

                bool fits = true;
                for (int i = 0; i < size; i++)
                {
                    int r = horizontal ? row : row + i;
                    int c = horizontal ? col + i : col;
                    if (ships[r, c]) { fits = false; break; }
                }

                if (fits)
                {
                    for (int i = 0; i < size; i++)
                    {
                        int r = horizontal ? row : row + i;
                        int c = horizontal ? col + i : col;
                        ships[r, c] = true;
                    }
                    placed = true;
                }
            }
        }

        // 3. Hier wird die 10 an dein Fenster übergeben!
        Board.Init(SIZE, SIZE, "Battleship", cellWidth: 50, cellHeight: 40, fontSize: 16);

        int hits = 0;
        int totalShipCells = 2 + 3 + 4 + 5; // 14 Treffepunkte
        int shots = 18; // Deine 15-20 Versuche (hier genau 18)

        while (shots > 0 && hits < totalShipCells)
        {
            Console.WriteLine($"Shots left: {shots} | Hits: {hits}/{totalShipCells}");

            var (row, col) = Board.WaitForClick();
            if (row < 0) return;

            string current = Board.GetText(row, col);
            if (current == "X" || current == "O")
            {
                Console.WriteLine("Already shot here!");
                continue;
            }

            if (ships[row, col])
            {
                Board.SetText(row, col, "X", "Red");
                hits++;
                Console.WriteLine("Hit!");
            }
            else
            {
                Board.SetText(row, col, "O", "Green");
                Console.WriteLine("Miss!");
            }
            shots--;
        }

        if (hits == totalShipCells)
            Console.WriteLine("You Win!");
        else
            Console.WriteLine("You Lose!");

        Console.WriteLine("Click anywhere to exit.");
        Board.WaitForClick();
        Board.Exit();
    }
}