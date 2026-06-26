using System;
using System.Threading;

namespace BoardTest;

public static class BoardDemo
{
    public static void Run()
    {
        const int SIZE = 10; 
        const int MAX_SHOTS = 50;
        bool[,] ships = new bool[SIZE, SIZE];

        int[] shipSizes = { 2, 3, 4, 5 };

        //macht die zufällige Platzierung von den Schiffen
        foreach (int size in shipSizes)
        {
            bool placed = false;
            while (!placed)
            {
                bool horizontal = Random.Shared.Next(2) == 0;
                int row = Random.Shared.Next(SIZE - (horizontal ? 0 : size));
                int col = Random.Shared.Next(SIZE - (horizontal ? size : 0));

                bool fits = true;
                //macht die richtung(horizontal oder vertikal)
                for (int i = 0; i < size; i++)
                {
                    int r = horizontal ? row : row + i;
                    int c = horizontal ? col + i : col;
                    //schaut ob anderes schiff da schon ist
                    if (ships[r, c])
                    {   
                        fits = false;
                        break; 
                    }
                }
                //wenn true macht es die schiffe an die richtige stelle
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

        Board.Init(SIZE, SIZE, "Battleship", cellWidth: 50, cellHeight: 40, fontSize: 16);

        int hits = 0;
        int totalShipCells = 2 + 3 + 4 + 5;
        int shots = MAX_SHOTS;

        while (shots > 0 && hits < totalShipCells)
        {
            Console.WriteLine($"Shots left: {shots} | Hits: {hits}/{totalShipCells}");

            var (row, col) = Board.WaitForClick();
            if (row < 0)
            {
                return;
            } 

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
                shots--; 
            }
        }

        if (hits == totalShipCells)
        {
            Console.WriteLine("You Win!");
        }
        else
        {
            Console.WriteLine("You Lose!");
        }
            

        Console.WriteLine("Click anywhere to exit.");
        Board.WaitForClick();
        Board.Exit();
    }
}