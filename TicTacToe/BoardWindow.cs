using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using System.Collections.Concurrent;

namespace BoardTest;

public class BoardWindow : Window
{
    private readonly TextBlock[,] _cells;
    private readonly int _rows;
    private readonly int _cols;
    private readonly int _cellWidth;
    private readonly int _cellHeight;
    private readonly int _fontSize;
    private readonly BlockingCollection<(int row, int col)> _clicks = new();

    public BoardWindow(int rows, int cols, string title, int cellWidth = 36, int cellHeight = 24, int fontSize = 11)
    {
        _rows = rows;
        _cols = cols;
        _cellWidth = cellWidth;
        _cellHeight = cellHeight;
        _fontSize = fontSize;
        _cells = new TextBlock[rows, cols];

        Title = title;
        CanResize = false;

        var grid = new Grid();

        int headerWidth  = Math.Max(cellWidth  / 2, 16);
        int headerHeight = Math.Max(cellHeight / 2, 14);

        // col 0 = row-header, cols 1..cols = data
        grid.ColumnDefinitions.Add(new ColumnDefinition(headerWidth, GridUnitType.Pixel));
        for (int c = 0; c < cols; c++)
            grid.ColumnDefinitions.Add(new ColumnDefinition(cellWidth, GridUnitType.Pixel));

        // row 0 = col-header, rows 1..rows = data
        grid.RowDefinitions.Add(new RowDefinition(headerHeight, GridUnitType.Pixel));
        for (int r = 0; r < rows; r++)
            grid.RowDefinitions.Add(new RowDefinition(cellHeight, GridUnitType.Pixel));

        for (int c = 0; c < cols; c++)
        {
            var (border, _) = MakeCell(c.ToString(), Brushes.SkyBlue, headerWidth, headerHeight, fontSize / 2);
            Grid.SetRow(border, 0);
            Grid.SetColumn(border, c + 1);
            grid.Children.Add(border);
        }

        for (int r = 0; r < rows; r++)
        {
            var (border, _) = MakeCell(r.ToString(), Brushes.SkyBlue, headerWidth, headerHeight, fontSize / 2);
            Grid.SetRow(border, r + 1);
            Grid.SetColumn(border, 0);
            grid.Children.Add(border);
        }

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int row = r, col = c; // capture loop vars for closures
                var (border, label) = MakeCell("", Brushes.White, cellWidth, cellHeight, fontSize);
                _cells[r, c] = label;
                border.Cursor = new Cursor(StandardCursorType.Hand);
                border.PointerPressed += (_, _) => _clicks.TryAdd((row, col));
                border.PointerEntered += (_, _) => border.Background = Brushes.LightYellow;
                border.PointerExited  += (_, _) => border.Background = Brushes.White;
                Grid.SetRow(border, r + 1);
                Grid.SetColumn(border, c + 1);
                grid.Children.Add(border);
            }
        }

        Content = grid;
        Width  = cols * cellWidth  + headerWidth  + 20;
        Height = rows * cellHeight + headerHeight + 50;
        Position = new PixelPoint(800, 50);

        Closed += (_, _) =>
        {
            _clicks.CompleteAdding();
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                desktop.Shutdown();
        };
    }

    private static (Border border, TextBlock label) MakeCell(
        string text, IBrush background, int width, int height, int fontSize)
    {
        var label = new TextBlock
        {
            Text = text,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = fontSize,
            Foreground = Brushes.Black,
        };
        var border = new Border
        {
            Width = width,
            Height = height,
            Background = background,
            BorderBrush = Brushes.Gray,
            BorderThickness = new Thickness(1),
            Child = label,
        };
        return (border, label);
    }

    public void SetCell(int row, int col, string text, string color)
    {
        _cells[row, col].Text = text;
        _cells[row, col].Foreground = color switch
        {
            "Red" => Brushes.Red,
            "Green" => Brushes.Green,
            _ => Brushes.Black,
        };
    }

    public string GetCell(int row, int col) => _cells[row, col].Text ?? string.Empty;

    // Blocks the calling thread until a cell is clicked; returns (-1,-1) if the window was closed.
    public (int row, int col) WaitForClick()
    {
        try { return _clicks.Take(); }
        catch (InvalidOperationException) { return (-1, -1); }
    }
}
