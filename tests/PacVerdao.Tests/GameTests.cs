using PacVerdao.Core;
using Xunit;

namespace PacVerdao.Tests;

public class GameTests
{
    private static Game NewPlayingGame(int phase = 0)
    {
        var game = new Game(seed: 42, startPhase: phase);
        while (game.State == GameState.Ready) game.Tick();
        Assert.Equal(GameState.Playing, game.State);
        return game;
    }

    private static void TickUntil(Game game, Func<bool> condition, int maxTicks = 5000)
    {
        for (int i = 0; i < maxTicks && !condition(); i++) game.Tick();
        Assert.True(condition(), "A condição esperada não aconteceu a tempo.");
    }

    [Fact]
    public void Starts_with_three_lives_in_the_group_stage()
    {
        var game = new Game(seed: 1);
        Assert.Equal(3, game.Lives);
        Assert.Equal(0, game.Score);
        Assert.Equal("FASE DE GRUPOS", game.Phase.Name);
        Assert.Equal(GameState.Ready, game.State);
    }

    [Fact]
    public void Campaign_goes_from_group_stage_to_the_final()
    {
        Assert.Equal(
            new[] { "FASE DE GRUPOS", "OITAVAS DE FINAL", "QUARTAS DE FINAL", "SEMIFINAL", "GRANDE FINAL" },
            Phases.All.Select(p => p.Name));
    }

    [Fact]
    public void Collecting_a_taca_scores_points()
    {
        Game game = NewPlayingGame();
        int before = game.Maze.TacasRemaining;

        // O Palmeiras começa andando para a esquerda, onde há taças.
        TickUntil(game, () => game.Maze.TacasRemaining < before);

        Assert.Equal(before - 1, game.Maze.TacasRemaining);
        Assert.Equal(Game.TacaPoints, game.Score);
    }

    [Fact]
    public void Player_turns_when_the_path_is_free()
    {
        Game game = NewPlayingGame();
        game.Player.Position = new Point(1, 13);
        game.Tick(Direction.Down);
        TickUntil(game, () => game.Player.Position != new Point(1, 13));
        Assert.Equal(new Point(1, 14), game.Player.Position);
        Assert.Equal(Direction.Down, game.Player.Direction);
    }

    [Fact]
    public void Walls_block_the_player()
    {
        Game game = NewPlayingGame();
        game.Player.Position = new Point(1, 19);
        game.Player.Direction = Direction.Down;
        for (int i = 0; i < 50; i++) game.Tick();
        Assert.Equal(new Point(1, 19), game.Player.Position);
        Assert.False(game.Player.IsMoving);
    }

    [Fact]
    public void Abel_ferreira_makes_rivals_vulnerable_and_they_can_be_defeated()
    {
        Game game = NewPlayingGame();
        TickUntil(game, () => game.Rivals[0].State == RivalState.Active);

        // Coloca o Palmeiras ao lado do Abel (canto inferior esquerdo) e anda até ele.
        game.Player.Position = new Point(2, 15);
        game.Player.Direction = Direction.Left;
        TickUntil(game, () => game.FrightenedTicks > 0);

        Rival flamengo = game.Rivals[0];
        Assert.True(flamengo.Frightened);
        Assert.Equal(GameState.Playing, game.State);

        int scoreBefore = game.Score;
        game.Player.Position = flamengo.Position;
        game.Player.Direction = Direction.None;
        game.Tick();

        Assert.Equal(RivalState.Eaten, flamengo.State);
        Assert.False(flamengo.Frightened);
        Assert.True(game.Score >= scoreBefore + Game.RivalBasePoints);
        Assert.Equal(3, game.Lives);
    }

    [Fact]
    public void Defeated_rival_goes_back_home_and_returns_to_play()
    {
        Game game = NewPlayingGame();
        TickUntil(game, () => game.Rivals[0].State == RivalState.Active);
        game.Player.Position = new Point(2, 15);
        game.Player.Direction = Direction.Left;
        TickUntil(game, () => game.FrightenedTicks > 0);

        Rival flamengo = game.Rivals[0];
        game.Player.Position = flamengo.Position;
        game.Player.Direction = Direction.None;
        game.Tick();
        Assert.Equal(RivalState.Eaten, flamengo.State);

        // Tira o Palmeiras do caminho para o rival voltar em paz.
        game.Player.Position = new Point(1, 19);
        TickUntil(game, () => flamengo.State == RivalState.Active);
        Assert.False(flamengo.Frightened);
    }

    [Fact]
    public void Being_caught_costs_a_life_and_three_catches_mean_serie_b()
    {
        Game game = NewPlayingGame();

        for (int life = 3; life >= 1; life--)
        {
            TickUntil(game, () => game.State == GameState.Playing);
            TickUntil(game, () => game.Rivals[0].State == RivalState.Active);

            game.Player.Position = game.Rivals[0].Position;
            game.Player.Direction = Direction.None;
            game.Tick();

            Assert.Equal(GameState.Dying, game.State);
            Assert.Equal(life - 1, game.Lives);
        }

        TickUntil(game, () => game.State != GameState.Dying);
        Assert.Equal(GameState.GameOver, game.State);
    }

    [Fact]
    public void After_losing_a_life_everyone_goes_back_to_the_start()
    {
        Game game = NewPlayingGame();
        TickUntil(game, () => game.Rivals[0].State == RivalState.Active);
        game.Player.Position = game.Rivals[0].Position;
        game.Player.Direction = Direction.None;
        game.Tick();

        TickUntil(game, () => game.State == GameState.Ready);
        Assert.Equal(game.Maze.PlayerStart, game.Player.Position);
        Assert.All(game.Rivals, r => Assert.Equal(RivalState.InHouse, r.State));
    }

    [Fact]
    public void Clearing_every_taca_advances_to_the_next_phase()
    {
        Game game = NewPlayingGame();
        EatAllButOne(game, out Point last);

        game.Player.Position = last;
        game.Player.Direction = Direction.None;
        // Coloca o Palmeiras numa casa vizinha e deixa ele andar até a última taça.
        StepOnto(game, last);

        TickUntil(game, () => game.State == GameState.PhaseComplete);
        game.StartNextPhase();
        Assert.Equal(1, game.PhaseIndex);
        Assert.Equal("OITAVAS DE FINAL", game.Phase.Name);
        Assert.Equal(GameState.Ready, game.State);
        Assert.Equal(game.Maze.TotalTacas, game.Maze.TacasRemaining);
    }

    [Fact]
    public void Winning_the_final_makes_palmeiras_champion()
    {
        Game game = NewPlayingGame(phase: Phases.All.Count - 1);
        EatAllButOne(game, out Point last);
        StepOnto(game, last);

        TickUntil(game, () => game.State == GameState.Victory);
        Assert.True(game.IsLastPhase);
    }

    /// <summary>Recolhe todas as taças menos uma, sem passar pelo movimento (atalho de teste).</summary>
    private static void EatAllButOne(Game game, out Point last)
    {
        var all = new List<Point>();
        for (int y = 0; y < game.Maze.Height; y++)
        for (int x = 0; x < game.Maze.Width; x++)
        {
            var p = new Point(x, y);
            if (game.Maze[p] == Tile.Taca) all.Add(p);
        }
        last = all[^1];
        foreach (Point p in all.Take(all.Count - 1)) game.Maze.Consume(p);
        for (int y = 0; y < game.Maze.Height; y++)
        for (int x = 0; x < game.Maze.Width; x++)
            if (game.Maze[new Point(x, y)] == Tile.Abel) game.Maze.Consume(new Point(x, y));
        Assert.Equal(1, game.Maze.TacasRemaining);
    }

    private static void StepOnto(Game game, Point target)
    {
        foreach (Direction d in DirectionExtensions.All)
        {
            Point from = game.Maze.Step(target, d.Opposite());
            if (!game.Maze.IsWalkable(from, allowDoor: false)) continue;
            game.Player.Position = from;
            game.Player.Direction = d;
            game.Player.NextDirection = d;
            return;
        }
        throw new InvalidOperationException("Sem casa vizinha livre.");
    }
}
