namespace PacVerdao.UI;

public readonly record struct Cell(char Char, ConsoleColor Foreground, ConsoleColor Background);

/// <summary>Tela em memória: tudo é desenhado aqui e depois enviado ao console de uma vez.</summary>
public sealed class Canvas
{
    public const int DefaultWidth = 80;
    public const int DefaultHeight = 24;

    private readonly Cell[] _cells;

    public Canvas(int width = DefaultWidth, int height = DefaultHeight)
    {
        Width = width;
        Height = height;
        _cells = new Cell[width * height];
        Clear();
    }

    public int Width { get; }
    public int Height { get; }

    public Cell this[int x, int y] => _cells[y * Width + x];

    public void Clear(ConsoleColor background = ConsoleColor.Black)
    {
        Array.Fill(_cells, new Cell(' ', ConsoleColor.Gray, background));
    }

    public void Put(int x, int y, string text, ConsoleColor fg, ConsoleColor bg = ConsoleColor.Black)
    {
        if (y < 0 || y >= Height) return;
        for (int i = 0; i < text.Length; i++)
        {
            int cx = x + i;
            if (cx < 0 || cx >= Width) continue;
            _cells[y * Width + cx] = new Cell(text[i], fg, bg);
        }
    }

    public void PutCentered(int y, string text, ConsoleColor fg, ConsoleColor bg = ConsoleColor.Black, int areaX = 0, int? areaWidth = null)
    {
        int width = areaWidth ?? Width;
        Put(areaX + Math.Max(0, (width - text.Length) / 2), y, text, fg, bg);
    }

    public void Fill(int x, int y, int width, int height, ConsoleColor bg)
    {
        for (int row = y; row < y + height; row++)
            Put(x, row, new string(' ', width), ConsoleColor.Gray, bg);
    }

    public void Box(int x, int y, int width, int height, ConsoleColor color)
    {
        Put(x, y, "+" + new string('-', width - 2) + "+", color);
        for (int row = y + 1; row < y + height - 1; row++)
        {
            Put(x, row, "|", color);
            Put(x + width - 1, row, "|", color);
        }
        Put(x, y + height - 1, "+" + new string('-', width - 2) + "+", color);
    }
}
