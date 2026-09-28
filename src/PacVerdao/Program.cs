using System.Diagnostics;
using System.Text;
using PacVerdao.Core;
using PacVerdao.UI;

namespace PacVerdao;

public static class Program
{
    public static int Main(string[] args)
    {
        int startPhase = 0;
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] is "--fase" or "-f" && i + 1 < args.Length &&
                int.TryParse(args[i + 1], out int phase) && phase >= 1 && phase <= Phases.All.Count)
            {
                startPhase = phase - 1;
            }
            else if (args[i] is "--help" or "-h")
            {
                Console.WriteLine("Pac-Verdão: a caça às taças da Libertadores");
                Console.WriteLine();
                Console.WriteLine("Uso: PacVerdao [--fase N]");
                Console.WriteLine($"  --fase N   começa direto na fase N (1 a {Phases.All.Count})");
                return 0;
            }
        }

        if (Console.IsInputRedirected || Console.IsOutputRedirected)
        {
            Console.Error.WriteLine("O Pac-Verdão precisa ser executado em um terminal interativo.");
            return 1;
        }

        Console.OutputEncoding = Encoding.UTF8;
        TryResizeWindow();
        Console.CursorVisible = false;
        Console.Clear();

        var app = new App(startPhase);
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            app.RequestQuit();
        };

        try
        {
            app.Run();
        }
        finally
        {
            Console.ResetColor();
            Console.Clear();
            Console.CursorVisible = true;
        }

        Console.WriteLine("Obrigado por jogar Pac-Verdão! Avanti, Palestra!");
        return 0;
    }

    private static void TryResizeWindow()
    {
        if (!OperatingSystem.IsWindows()) return;
        try
        {
            if (Console.WindowWidth < Canvas.DefaultWidth || Console.WindowHeight < Canvas.DefaultHeight)
            {
                Console.SetWindowSize(
                    Math.Max(Console.WindowWidth, Canvas.DefaultWidth),
                    Math.Max(Console.WindowHeight, Canvas.DefaultHeight));
            }
        }
        catch (Exception e) when (e is IOException or ArgumentOutOfRangeException or PlatformNotSupportedException)
        {
            // Alguns terminais não deixam redimensionar; o renderizador avisa se a janela for pequena.
        }
    }
}

/// <summary>Fluxo do jogo: título → fases → Série B ou título da Libertadores → de novo.</summary>
internal sealed class App
{
    private const int TickMilliseconds = 1000 / Game.TicksPerSecond;
    private const int FrameMilliseconds = 16;

    private readonly int _startPhase;
    private readonly Canvas _canvas = new();
    private readonly ConsoleRenderer _renderer = new();
    private int _highScore = HighScoreStore.Load();
    private volatile bool _quit;

    public App(int startPhase) => _startPhase = startPhase;

    public void RequestQuit() => _quit = true;

    public void Run()
    {
        if (!RunScreen(t => Screens.Title(_canvas, t, _highScore))) return;

        while (!_quit)
        {
            var game = new Game(startPhase: _startPhase);
            if (!PlayCampaign(game)) return;

            bool newRecord = game.Score > _highScore;
            if (newRecord)
            {
                _highScore = game.Score;
                HighScoreStore.Save(_highScore);
            }

            int seed = Environment.TickCount;
            bool again = game.State == GameState.Victory
                ? RunScreen(t => Screens.Champion(_canvas, game, t, newRecord, seed))
                : RunScreen(t => Screens.Relegated(_canvas, game, t, newRecord));
            if (!again) return;
        }
    }

    /// <returns>false se o jogador pediu para sair no meio da campanha.</returns>
    private bool PlayCampaign(Game game)
    {
        while (!_quit)
        {
            if (!RunScreen(t => Screens.PhaseIntro(_canvas, game, t), autoContinueTicks: 5 * Game.TicksPerSecond))
                return false;
            if (!PlayPhase(game)) return false;

            if (game.State != GameState.PhaseComplete) return true; // Série B ou campeão.
            game.StartNextPhase();
        }
        return false;
    }

    /// <summary>Roda uma fase até ela terminar. Retorna false se o jogador sair.</summary>
    private bool PlayPhase(Game game)
    {
        var clock = Stopwatch.StartNew();
        long nextTick = 0;
        bool paused = false;
        bool quitPrompt = false;
        Direction pending = Direction.None;

        while (!_quit)
        {
            InputSnapshot input = InputHandler.Poll();
            if (quitPrompt)
            {
                if (input.Quit) return false;
                if (input.Confirm || input.Pause) quitPrompt = paused = false;
            }
            else if (input.Quit)
            {
                quitPrompt = paused = true;
            }
            else if (input.Pause)
            {
                paused = !paused;
            }

            if (input.Direction != Direction.None) pending = input.Direction;

            long now = clock.ElapsedMilliseconds;
            if (paused)
            {
                nextTick = now;
            }
            else
            {
                // Se o computador engasgar, não tenta "recuperar" segundos inteiros de jogo.
                if (now - nextTick > 250) nextTick = now;
                while (now >= nextTick && !IsFinished(game))
                {
                    game.Tick(pending);
                    pending = Direction.None;
                    nextTick += TickMilliseconds;
                }
            }

            GameView.Draw(_canvas, game, _highScore, paused && !quitPrompt, quitPrompt);
            _renderer.Present(_canvas);

            if (IsFinished(game)) return true;
            Thread.Sleep(FrameMilliseconds);
        }
        return false;
    }

    private static bool IsFinished(Game game) =>
        game.State is GameState.PhaseComplete or GameState.GameOver or GameState.Victory;

    /// <summary>Mostra uma tela animada até ENTER (true) ou ESC (false).</summary>
    private bool RunScreen(Action<long> draw, int? autoContinueTicks = null)
    {
        var clock = Stopwatch.StartNew();
        // Descarta teclas apertadas durante a partida para não pular a tela sem querer.
        InputHandler.Poll();

        while (!_quit)
        {
            long t = clock.ElapsedMilliseconds / TickMilliseconds;
            draw(t);
            _renderer.Present(_canvas);

            InputSnapshot input = InputHandler.Poll();
            if (input.Quit) return false;
            if (input.Confirm) return true;
            if (autoContinueTicks is int limit && t >= limit) return true;
            Thread.Sleep(FrameMilliseconds);
        }
        return false;
    }
}
