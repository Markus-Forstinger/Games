using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using System.Threading;

namespace BoardTest;

public class Board
{
    static volatile bool _boardInitialized;
    static int _rows;
    static int _cols;
    static BoardWindow? _window;

    public static void Init(int rows, int cols, string title,
                            int cellWidth = 36, int cellHeight = 24, int fontSize = 11)
    {
        _rows = Math.Max(0, rows);
        _cols = Math.Max(0, cols);
        _boardInitialized = false;

        App.Ready.Wait();

        Dispatcher.UIThread.Post(() =>
        {
            _window = new BoardWindow(_rows, _cols, title, cellWidth, cellHeight, fontSize);
            _window.Show();
            _boardInitialized = true;
        });

        while (!_boardInitialized)
            Thread.Sleep(10);
    }

    public static void Exit()
    {
        Dispatcher.UIThread.Post(() =>
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                desktop.Shutdown();
        });
    }

    public static void SetText(int row, int col, string text, string color = "Black")
    {
        if (_window == null) return;
        if (row < 0 || row >= _rows || col < 0 || col >= _cols) return;
        Dispatcher.UIThread.Post(() => _window.SetCell(row, col, text, color));
    }

    public static void SetText(int row, int col, string text) => SetText(row, col, text, "Black");

    public static string? GetText(int row, int col)
    {
        if (_window == null) return null;
        if (row < 0 || row >= _rows || col < 0 || col >= _cols) return null;
        string? result = null;
        var done = new ManualResetEventSlim(false);
        Dispatcher.UIThread.Post(() =>
        {
            result = _window.GetCell(row, col);
            done.Set();
        });
        done.Wait();
        return result;
    }

    public static void Clear()
    {
        if (_window == null) return;
        for (int r = 0; r < _rows; r++)
            for (int c = 0; c < _cols; c++)
                SetText(r, c, "");
    }

    public static int GetLength(int dimension)
    {
        if (dimension < 0 || dimension >= 2) return -1;
        return dimension == 0 ? _rows : _cols;
    }

    // Blocks until the user clicks a cell; returns (-1,-1) if the window was closed.
    public static (int row, int col) WaitForClick()
    {
        if (_window == null) return (-1, -1);
        return _window.WaitForClick();
    }
}
