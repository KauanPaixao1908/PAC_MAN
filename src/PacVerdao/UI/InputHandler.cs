using PacVerdao.Core;

namespace PacVerdao.UI;

public readonly record struct InputSnapshot(Direction Direction, bool Pause, bool Quit, bool Confirm);

/// <summary>Lê o teclado sem bloquear: setas/WASD movem, P pausa, ESC sai, ENTER/Espaço confirma.</summary>
public static class InputHandler
{
    public static InputSnapshot Poll()
    {
        Direction direction = Direction.None;
        bool pause = false, quit = false, confirm = false;

        while (Console.KeyAvailable)
        {
            ConsoleKeyInfo key = Console.ReadKey(intercept: true);
            switch (key.Key)
            {
                case ConsoleKey.UpArrow or ConsoleKey.W or ConsoleKey.NumPad8:
                    direction = Direction.Up;
                    break;
                case ConsoleKey.DownArrow or ConsoleKey.S or ConsoleKey.NumPad2:
                    direction = Direction.Down;
                    break;
                case ConsoleKey.LeftArrow or ConsoleKey.A or ConsoleKey.NumPad4:
                    direction = Direction.Left;
                    break;
                case ConsoleKey.RightArrow or ConsoleKey.D or ConsoleKey.NumPad6:
                    direction = Direction.Right;
                    break;
                case ConsoleKey.P:
                    pause = true;
                    break;
                case ConsoleKey.Escape or ConsoleKey.Q:
                    quit = true;
                    break;
                case ConsoleKey.Enter or ConsoleKey.Spacebar:
                    confirm = true;
                    break;
            }
        }

        return new InputSnapshot(direction, pause, quit, confirm);
    }
}
