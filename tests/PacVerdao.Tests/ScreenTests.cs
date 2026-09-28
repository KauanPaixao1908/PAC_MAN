using System.Text;
using PacVerdao.Core;
using PacVerdao.UI;
using Xunit;

namespace PacVerdao.Tests;

public class ScreenTests
{
    [Fact]
    public void Every_screen_fits_the_canvas_and_shows_its_message()
    {
        var canvas = new Canvas();
        var game = new Game(seed: 7);

        Screens.Title(canvas, 0, highScore: 1234);
        Assert.Contains("RECORDE: 001234", Text(canvas));

        Screens.PhaseIntro(canvas, game, 0);
        Assert.Contains("FASE DE GRUPOS", Text(canvas));

        Screens.Relegated(canvas, game, 0, newRecord: false);
        Assert.Contains("SÉRIE B", Text(canvas));

        Screens.Champion(canvas, game, 0, newRecord: true, seed: 1);
        Assert.Contains("O MAIOR TIME DO UNIVERSO!", Text(canvas));
    }

    [Fact]
    public void Game_view_draws_player_rivals_and_panel()
    {
        var canvas = new Canvas();
        var game = new Game(seed: 7);
        for (int i = 0; i < 300; i++)
        {
            game.Tick(Direction.Left);
            GameView.Draw(canvas, game, highScore: 0, paused: false, quitPrompt: false);
        }

        string text = Text(canvas);
        Assert.Contains("PA", text);
        Assert.Contains("FL", text);
        Assert.Contains("COPA LIBERTADORES", text);
        Assert.Contains("[>] Fase de Grupos", text);
    }

    internal static string Text(Canvas canvas)
    {
        var sb = new StringBuilder();
        for (int y = 0; y < canvas.Height; y++)
        {
            for (int x = 0; x < canvas.Width; x++) sb.Append(canvas[x, y].Char);
            sb.AppendLine();
        }
        return sb.ToString();
    }
}
