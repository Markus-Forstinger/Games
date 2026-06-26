using System.Threading;

namespace BoardTest;

public static class BoardDemo
{
    public static void Run()
    {
        //macht das Board
        Board.Init(3, 3, "Tic-Tac-Toe", cellWidth: 80, cellHeight: 70, fontSize: 32);
        
        string currentPlayer = "X";
        string playerColor = "Red";
        int moves = 0;
        bool isGameActive = true;

        //das Spiel 1v1 TicTacToe
        while (isGameActive)
        {
            var (row, col) = Board.WaitForClick();

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
        //für jede reihe schauen
        for (int row = 0; row < 3; row++)
        {
            if (Board.GetText(row, 0) == player && Board.GetText(row, 1) == player && Board.GetText(row, 2) == player)
            {
                return true;
            }
        }
        //für jede Spalte
        for (int column = 0; column < 3; column++)
        {
            if (Board.GetText(0, column) == player && Board.GetText(1, column) == player && Board.GetText(2, column) == player)
            {
                return true;
            }
        }
        //für diagonale 1
        if (Board.GetText(0, 0) == player && Board.GetText(1, 1) == player && Board.GetText(2, 2) == player)
        {
            return true;
        }
        //für dieagonale 2
        if (Board.GetText(0, 2) == player && Board.GetText(1, 1) == player && Board.GetText(2, 0) == player)
        {
            return true;
        }

        return false;
    }
}


