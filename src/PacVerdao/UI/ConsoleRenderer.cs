using System.Text;

namespace PacVerdao.UI;

/// <summary>
/// Envia o <see cref="Canvas"/> para o console redesenhando só o que mudou, o que evita o
/// "pisca-pisca" típico de jogos de console.
/// </summary>
public sealed class ConsoleRenderer
{
    private Cell[]? _previous;
    private int _lastWindowWidth = -1;
    private int _lastWindowHeight = -1;
    private readonly StringBuilder _run = new();

    public void Present(Canvas canvas)
    {
        int windowWidth = SafeWindowWidth();
        int windowHeight = SafeWindowHeight();

        if (windowWidth != _lastWindowWidth || windowHeight != _lastWindowHeight)
        {
            _lastWindowWidth = windowWidth;
            _lastWindowHeight = windowHeight;
            _previous = null;
            Console.ResetColor();
            Console.Clear();
        }

        if (windowWidth < canvas.Width || windowHeight < canvas.Height)
        {
            if (_previous is null)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write($"Aumente a janela para pelo menos {canvas.Width}x{canvas.Height} (atual: {windowWidth}x{windowHeight}).");
                _previous = Array.Empty<Cell>();
            }
            return;
        }

        bool full = _previous is null || _previous.Length != canvas.Width * canvas.Height;
        if (full) _previous = new Cell[canvas.Width * canvas.Height];
        Cell[] previous = _previous!;

        for (int y = 0; y < canvas.Height; y++)
        {
            int x = 0;
            // Nunca escreve na última coluna da última linha, para o console não rolar.
            int maxX = y == canvas.Height - 1 ? canvas.Width - 1 : canvas.Width;
            while (x < maxX)
            {
                Cell cell = canvas[x, y];
                int index = y * canvas.Width + x;
                if (!full && previous[index] == cell)
                {
                    x++;
                    continue;
                }

                int start = x;
                _run.Clear();
                while (x < maxX)
                {
                    Cell c = canvas[x, y];
                    int i = y * canvas.Width + x;
                    if (c.Foreground != cell.Foreground || c.Background != cell.Background) break;
                    if (!full && previous[i] == c) break;
                    _run.Append(c.Char);
                    previous[i] = c;
                    x++;
                }

                Console.SetCursorPosition(start, y);
                Console.ForegroundColor = cell.Foreground;
                Console.BackgroundColor = cell.Background;
                Console.Write(_run.ToString());
            }
        }
        Console.ResetColor();
    }

    public void Invalidate() => _previous = null;

    private static int SafeWindowWidth()
    {
        try { return Console.WindowWidth; } catch (IOException) { return Canvas.DefaultWidth; }
    }

    private static int SafeWindowHeight()
    {
        try { return Console.WindowHeight; } catch (IOException) { return Canvas.DefaultHeight; }
    }
}
