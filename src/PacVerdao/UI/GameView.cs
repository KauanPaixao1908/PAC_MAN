using PacVerdao.Core;

namespace PacVerdao.UI;

/// <summary>Desenha a partida: placar, campo, rivais e o painel lateral da Libertadores.</summary>
public static class GameView
{
    public const int FieldTop = 1;
    public const int PanelX = 58;

    private static readonly string[] ShortPhaseNames = { "Fase de Grupos", "Oitavas", "Quartas", "Semifinal", "Final" };

    private static readonly ConsoleColor[] WallColors =
    {
        ConsoleColor.DarkGreen,
        ConsoleColor.DarkCyan,
        ConsoleColor.DarkBlue,
        ConsoleColor.DarkMagenta,
        ConsoleColor.DarkYellow,
    };

    public static void Draw(Canvas canvas, Game game, int highScore, bool paused, bool quitPrompt)
    {
        canvas.Clear();
        long t = game.TotalTicks;

        DrawHud(canvas, game, highScore);
        DrawField(canvas, game, t);
        DrawRivals(canvas, game, t);
        DrawPlayer(canvas, game, t);

        foreach (Popup popup in game.Popups)
            canvas.Put(popup.Position.X * 2, FieldTop + popup.Position.Y, popup.Text, ConsoleColor.Cyan);

        int messageY = FieldTop + 11;
        int fieldWidth = game.Maze.Width * 2;
        if (quitPrompt)
            canvas.PutCentered(messageY, " SAIR? ESC=sim  ENTER=não ", ConsoleColor.Black, ConsoleColor.Yellow, 0, fieldWidth);
        else if (paused)
            canvas.PutCentered(messageY, " PAUSADO ", ConsoleColor.Black, ConsoleColor.Yellow, 0, fieldWidth);
        else if (game.State == GameState.Ready)
            canvas.PutCentered(messageY, "PRONTO!", ConsoleColor.Yellow, ConsoleColor.Black, 0, fieldWidth);

        if (game.Banner is not null)
            canvas.PutCentered(FieldTop + game.Maze.Height, game.Banner, ConsoleColor.White, ConsoleColor.Black, 0, fieldWidth);
        canvas.Put(0, canvas.Height - 1, "Setas/WASD: mover   P: pausar   ESC: sair", ConsoleColor.DarkGray);

        DrawPanel(canvas, game);
    }

    private static void DrawHud(Canvas canvas, Game game, int highScore)
    {
        canvas.Put(0, 0, "PLACAR ", ConsoleColor.Gray);
        canvas.Put(7, 0, game.Score.ToString("D6"), ConsoleColor.White);
        canvas.Put(18, 0, "RECORDE ", ConsoleColor.Gray);
        canvas.Put(26, 0, Math.Max(highScore, game.Score).ToString("D6"), ConsoleColor.White);
        canvas.Put(38, 0, "VIDAS ", ConsoleColor.Gray);
        for (int i = 0; i < Game.InitialLives; i++)
        {
            bool alive = i < game.Lives;
            canvas.Put(44 + i * 3, 0, "PA", alive ? ConsoleColor.White : ConsoleColor.DarkGray,
                alive ? ConsoleColor.DarkGreen : ConsoleColor.Black);
        }
    }

    private static void DrawField(Canvas canvas, Game game, long t)
    {
        Maze maze = game.Maze;
        ConsoleColor wallColor = WallColors[game.PhaseIndex % WallColors.Length];
        if (game.State == GameState.PhaseCleared && t / 10 % 2 == 0) wallColor = ConsoleColor.White;
        bool abelBlink = t / 12 % 2 == 0;

        for (int y = 0; y < maze.Height; y++)
        {
            for (int x = 0; x < maze.Width; x++)
            {
                int sx = x * 2;
                int sy = FieldTop + y;
                switch (maze[new Point(x, y)])
                {
                    case Tile.Wall:
                        canvas.Put(sx, sy, "  ", wallColor, wallColor);
                        break;
                    case Tile.Door:
                        canvas.Put(sx, sy, "--", ConsoleColor.White);
                        break;
                    case Tile.Taca:
                        canvas.Put(sx, sy, "• ", ConsoleColor.Yellow);
                        break;
                    case Tile.Abel:
                        canvas.Put(sx, sy, "AB", abelBlink ? ConsoleColor.Black : ConsoleColor.Yellow,
                            abelBlink ? ConsoleColor.Yellow : ConsoleColor.Black);
                        break;
                }
            }
        }
    }

    private static void DrawRivals(Canvas canvas, Game game, long t)
    {
        if (game.State is GameState.PhaseCleared or GameState.Victory) return;
        if (game.State == GameState.Dying && game.StateTicks < 50) return;

        bool endingSoon = game.FrightenedTicks > 0 && game.FrightenedTicks < 2 * Game.TicksPerSecond;
        foreach (Rival rival in game.Rivals)
        {
            int sx = rival.Position.X * 2;
            int sy = FieldTop + rival.Position.Y;

            if (rival.State == RivalState.Eaten)
            {
                canvas.Put(sx, sy, "oo", ConsoleColor.White);
            }
            else if (rival.Frightened)
            {
                bool flash = endingSoon && t / 8 % 2 == 0;
                canvas.Put(sx, sy, rival.Tag.ToLowerInvariant(),
                    flash ? ConsoleColor.Blue : ConsoleColor.White,
                    flash ? ConsoleColor.White : ConsoleColor.DarkBlue);
            }
            else
            {
                (ConsoleColor fg, ConsoleColor bg) = RivalColors(rival.Kind);
                canvas.Put(sx, sy, rival.Tag, fg, bg);
            }
        }
    }

    public static (ConsoleColor Fg, ConsoleColor Bg) RivalColors(RivalKind kind) => kind switch
    {
        RivalKind.Flamengo => (ConsoleColor.Black, ConsoleColor.Red),
        RivalKind.Corinthians => (ConsoleColor.White, ConsoleColor.DarkGray),
        _ => (ConsoleColor.Red, ConsoleColor.White),
    };

    private static void DrawPlayer(Canvas canvas, Game game, long t)
    {
        int sx = game.Player.Position.X * 2;
        int sy = FieldTop + game.Player.Position.Y;

        if (game.State == GameState.Dying || game.State == GameState.GameOver)
        {
            // Animação de derrota: pisca e vira um "XX".
            if (game.StateTicks > 25 && t / 6 % 2 == 0)
                canvas.Put(sx, sy, "PA", ConsoleColor.White, ConsoleColor.DarkGreen);
            else if (game.StateTicks <= 25)
                canvas.Put(sx, sy, "XX", ConsoleColor.Red);
            return;
        }

        // "Mastigando" as taças: alterna o tom de verde enquanto anda.
        bool chomp = game.Player.IsMoving && t / 5 % 2 == 0;
        canvas.Put(sx, sy, "PA", ConsoleColor.White, chomp ? ConsoleColor.Green : ConsoleColor.DarkGreen);
    }

    private static void DrawPanel(Canvas canvas, Game game)
    {
        int x = PanelX;
        canvas.Put(x, 1, "COPA LIBERTADORES", ConsoleColor.Yellow);
        canvas.Put(x, 2, "-----------------", ConsoleColor.DarkYellow);
        for (int i = 0; i < ShortPhaseNames.Length; i++)
        {
            bool done = i < game.PhaseIndex || (i == game.PhaseIndex && game.State is GameState.PhaseCleared or GameState.PhaseComplete or GameState.Victory);
            bool current = i == game.PhaseIndex && !done;
            string mark = done ? "[x] " : current ? "[>] " : "[ ] ";
            ConsoleColor color = done ? ConsoleColor.DarkGreen : current ? ConsoleColor.Green : ConsoleColor.DarkGray;
            canvas.Put(x, 3 + i, mark + ShortPhaseNames[i], color);
        }

        canvas.Put(x, 9, "TAÇAS ", ConsoleColor.Gray);
        canvas.Put(x + 6, 9, $"{game.Maze.TacasRemaining}/{game.Maze.TotalTacas}", ConsoleColor.Yellow);

        canvas.Put(x, 11, "RIVAIS", ConsoleColor.Gray);
        int row = 12;
        foreach (RivalKind kind in new[] { RivalKind.Flamengo, RivalKind.Corinthians, RivalKind.SaoPaulo })
        {
            (ConsoleColor fg, ConsoleColor bg) = RivalColors(kind);
            string tag = kind switch { RivalKind.Flamengo => "FL", RivalKind.Corinthians => "CO", _ => "SP" };
            string name = kind switch { RivalKind.Flamengo => "Flamengo", RivalKind.Corinthians => "Corinthians", _ => "São Paulo" };
            canvas.Put(x, row, tag, fg, bg);
            canvas.Put(x + 3, row, name, ConsoleColor.Gray);
            row++;
        }

        canvas.Put(x, 16, "AB", ConsoleColor.Black, ConsoleColor.Yellow);
        canvas.Put(x + 3, 16, "Abel Ferreira", ConsoleColor.Gray);
        canvas.Put(x, 17, "• ", ConsoleColor.Yellow);
        canvas.Put(x + 3, 17, "Taça (10 pts)", ConsoleColor.Gray);

        if (game.FrightenedTicks > 0)
        {
            int seconds = (game.FrightenedTicks + Game.TicksPerSecond - 1) / Game.TicksPerSecond;
            canvas.Put(x, 19, $"ABEL EM CAMPO: {seconds}s", ConsoleColor.Yellow);
        }
        else if (game.State == GameState.Playing)
        {
            bool chase = game.Mode == RivalMode.Chase;
            canvas.Put(x, 19, chase ? "Rivais: PRESSIONANDO" : "Rivais: recuados", chase ? ConsoleColor.Red : ConsoleColor.DarkGray);
        }

        canvas.Put(x, 21, $"Fase {game.PhaseIndex + 1} de {Phases.All.Count}", ConsoleColor.DarkGray);
    }
}
