namespace PacVerdao.Core;

public enum GameState
{
    /// <summary>"PRONTO!" antes do apito inicial.</summary>
    Ready,
    Playing,
    /// <summary>O Palmeiras foi pego por um rival.</summary>
    Dying,
    /// <summary>Todas as taças da fase foram coletadas (animação de comemoração).</summary>
    PhaseCleared,
    /// <summary>Fase concluída; aguardando <see cref="Game.StartNextPhase"/>.</summary>
    PhaseComplete,
    /// <summary>Acabaram as vidas: rebaixado para a Série B.</summary>
    GameOver,
    /// <summary>Campeão da Libertadores: o maior time do universo.</summary>
    Victory,
}

public enum RivalMode
{
    Scatter,
    Chase,
}

/// <summary>
/// Regras do jogo, sem nenhuma dependência de console. Avance o jogo chamando <see cref="Tick"/>
/// <see cref="TicksPerSecond"/> vezes por segundo.
/// </summary>
public sealed class Game
{
    public const int TicksPerSecond = 50;
    public const int InitialLives = 3;
    public const int TacaPoints = 10;
    public const int AbelPoints = 50;
    public const int RivalBasePoints = 200;

    private const int ReadyTicks = 2 * TicksPerSecond;
    private const int DyingTicks = (int)(1.5 * TicksPerSecond);
    private const int ClearedTicks = 2 * TicksPerSecond;
    private const int BannerTicks = 2 * TicksPerSecond;
    private const int PopupTicks = TicksPerSecond;
    private const int EatenStep = 3;

    /// <summary>Alternância dispersar/perseguir dos rivais (o último modo dura para sempre).</summary>
    private static readonly (RivalMode Mode, int Ticks)[] ModeSchedule =
    {
        (RivalMode.Scatter, 7 * TicksPerSecond),
        (RivalMode.Chase, 20 * TicksPerSecond),
        (RivalMode.Scatter, 7 * TicksPerSecond),
        (RivalMode.Chase, 20 * TicksPerSecond),
        (RivalMode.Scatter, 5 * TicksPerSecond),
        (RivalMode.Chase, int.MaxValue),
    };

    private readonly Random _random;
    private readonly List<Rival> _rivals = new();
    private readonly List<Popup> _popups = new();
    private int _modeIndex;
    private int _modeTicks;
    private int _playTicks;
    private int _rivalsEatenCombo;

    public Game(int? seed = null, int startPhase = 0)
    {
        _random = seed is null ? new Random() : new Random(seed.Value);
        Lives = InitialLives;
        StartPhase(startPhase);
    }

    public Maze Maze { get; private set; } = null!;
    public Player Player { get; } = new();
    public IReadOnlyList<Rival> Rivals => _rivals;
    public IReadOnlyList<Popup> Popups => _popups;

    public int Score { get; private set; }
    public int Lives { get; private set; }
    public int PhaseIndex { get; private set; }
    public Phase Phase => Phases.All[PhaseIndex];
    public bool IsLastPhase => PhaseIndex == Phases.All.Count - 1;

    public GameState State { get; private set; }

    /// <summary>Ticks restantes no estado atual (Ready, Dying, PhaseCleared).</summary>
    public int StateTicks { get; private set; }

    /// <summary>Ticks restantes do Abel Ferreira em campo (0 = sem power-up).</summary>
    public int FrightenedTicks { get; private set; }

    public RivalMode Mode => ModeSchedule[_modeIndex].Mode;

    /// <summary>Contador global de ticks, útil para animações.</summary>
    public long TotalTicks { get; private set; }

    public string? Banner { get; private set; }
    public int BannerTicksLeft { get; private set; }

    public void StartPhase(int index)
    {
        if (index < 0 || index >= Phases.All.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        PhaseIndex = index;
        Maze = Maze.Parse(Phase.Layout);
        ResetActors();
        ShowBanner($"{Phase.Name} - {Phase.Stadium}");
    }

    public void StartNextPhase()
    {
        if (State != GameState.PhaseComplete)
            throw new InvalidOperationException("A fase atual ainda não terminou.");
        StartPhase(PhaseIndex + 1);
    }

    /// <summary>Avança o jogo um tick. <paramref name="input"/> é a direção pedida neste tick (ou None).</summary>
    public void Tick(Direction input = Direction.None)
    {
        TotalTicks++;
        UpdateEffects();

        switch (State)
        {
            case GameState.Ready:
                if (input != Direction.None) Player.NextDirection = input;
                if (--StateTicks <= 0) State = GameState.Playing;
                break;

            case GameState.Playing:
                TickPlaying(input);
                break;

            case GameState.Dying:
                if (--StateTicks <= 0)
                {
                    if (Lives <= 0)
                    {
                        State = GameState.GameOver;
                    }
                    else
                    {
                        ResetActors();
                    }
                }
                break;

            case GameState.PhaseCleared:
                if (--StateTicks <= 0)
                    State = IsLastPhase ? GameState.Victory : GameState.PhaseComplete;
                break;
        }
    }

    private void ResetActors()
    {
        Player.Position = Maze.PlayerStart;
        Player.Direction = Direction.Left;
        Player.NextDirection = Direction.None;
        Player.IsMoving = false;
        Player.MoveCounter = 0;

        _rivals.Clear();
        var kinds = new[] { RivalKind.Flamengo, RivalKind.Corinthians, RivalKind.SaoPaulo };
        var scatterTargets = new[]
        {
            new Point(Maze.Width - 3, -3), // Flamengo: canto superior direito
            new Point(2, -3),              // Corinthians: canto superior esquerdo
            new Point(0, Maze.Height),     // São Paulo: canto inferior esquerdo
        };
        for (int i = 0; i < kinds.Length; i++)
        {
            Point home = Maze.RivalStarts[i % Maze.RivalStarts.Count];
            _rivals.Add(new Rival(kinds[i], home, scatterTargets[i], i));
        }

        FrightenedTicks = 0;
        _modeIndex = 0;
        _modeTicks = 0;
        _playTicks = 0;
        _popups.Clear();
        State = GameState.Ready;
        StateTicks = ReadyTicks;
    }

    private void TickPlaying(Direction input)
    {
        if (input != Direction.None) Player.NextDirection = input;
        _playTicks++;

        if (CheckCollisions()) return;
        UpdateModes();

        if (FrightenedTicks > 0 && --FrightenedTicks == 0)
        {
            foreach (Rival r in _rivals) r.Frightened = false;
        }

        if (++Player.MoveCounter >= Phase.PlayerStep)
        {
            Player.MoveCounter = 0;
            MovePlayer();
            if (CheckCollisions()) return;
            if (Maze.TacasRemaining == 0)
            {
                State = GameState.PhaseCleared;
                StateTicks = ClearedTicks;
                ShowBanner(IsLastPhase ? "É CAMPEÃO!!!" : $"{Phase.Name} CONCLUÍDA!");
                return;
            }
        }

        foreach (Rival rival in _rivals)
        {
            if (rival.State == RivalState.InHouse && _playTicks >= Phase.ReleaseTicks[rival.ReleaseIndex])
                rival.State = RivalState.Leaving;

            if (++rival.MoveCounter >= RivalStep(rival))
            {
                rival.MoveCounter = 0;
                MoveRival(rival);
                if (CheckCollisions()) return;
            }
        }
    }

    private void UpdateModes()
    {
        // Enquanto o Abel está em campo o relógio tático dos rivais fica parado.
        if (FrightenedTicks > 0) return;
        if (++_modeTicks < ModeSchedule[_modeIndex].Ticks) return;

        _modeIndex = Math.Min(_modeIndex + 1, ModeSchedule.Length - 1);
        _modeTicks = 0;
        foreach (Rival r in _rivals)
        {
            if (r.State == RivalState.Active) r.ReverseRequested = true;
        }
    }

    private void UpdateEffects()
    {
        if (BannerTicksLeft > 0 && --BannerTicksLeft == 0) Banner = null;
        for (int i = _popups.Count - 1; i >= 0; i--)
        {
            if (--_popups[i].TicksLeft <= 0) _popups.RemoveAt(i);
        }
    }

    private void ShowBanner(string text)
    {
        Banner = text;
        BannerTicksLeft = BannerTicks;
    }

    private void MovePlayer()
    {
        if (Maze.CanMove(Player.Position, Player.NextDirection, allowDoor: false))
            Player.Direction = Player.NextDirection;

        if (!Maze.CanMove(Player.Position, Player.Direction, allowDoor: false))
        {
            Player.IsMoving = false;
            return;
        }

        Player.Position = Maze.Step(Player.Position, Player.Direction);
        Player.IsMoving = true;

        switch (Maze.Consume(Player.Position))
        {
            case Tile.Taca:
                Score += TacaPoints;
                break;
            case Tile.Abel:
                Score += AbelPoints;
                CallAbel();
                break;
        }
    }

    /// <summary>Power-up: o Abel Ferreira entra em campo e os rivais ficam vulneráveis.</summary>
    private void CallAbel()
    {
        FrightenedTicks = Phase.FrightenedTicks;
        _rivalsEatenCombo = 0;
        foreach (Rival r in _rivals)
        {
            if (r.State == RivalState.Eaten) continue;
            r.Frightened = true;
            if (r.State == RivalState.Active) r.ReverseRequested = true;
        }
        ShowBanner("ABEL FERREIRA ENTROU EM CAMPO!");
    }

    private int RivalStep(Rival rival) => rival.State switch
    {
        RivalState.Eaten => EatenStep,
        _ when rival.Frightened => Phase.FrightenedStep,
        RivalState.InHouse or RivalState.Leaving => Phase.RivalStep + 2,
        _ => Phase.RivalStep,
    };

    private void MoveRival(Rival rival)
    {
        switch (rival.State)
        {
            case RivalState.InHouse:
                return;

            case RivalState.Leaving:
                StepTowards(rival, Maze.HouseExit);
                if (rival.Position == Maze.HouseExit)
                {
                    rival.State = RivalState.Active;
                    rival.Direction = Direction.Left;
                }
                return;

            case RivalState.Eaten:
                StepTowards(rival, rival.Home);
                if (rival.Position == rival.Home)
                {
                    rival.State = RivalState.Leaving;
                    rival.Frightened = false;
                }
                return;

            case RivalState.Active:
                Direction dir;
                if (rival.ReverseRequested)
                {
                    rival.ReverseRequested = false;
                    dir = rival.Direction.Opposite();
                    if (!Maze.CanMove(rival.Position, dir, allowDoor: false)) dir = ChooseDirection(rival);
                }
                else
                {
                    dir = ChooseDirection(rival);
                }

                if (dir == Direction.None) return;
                rival.Direction = dir;
                rival.Position = Maze.Step(rival.Position, dir);
                return;
        }
    }

    private void StepTowards(Rival rival, Point target)
    {
        Direction dir = Maze.NextStepTowards(rival.Position, target, allowDoor: true);
        if (dir == Direction.None) return;
        rival.Direction = dir;
        rival.Position = Maze.Step(rival.Position, dir);
    }

    private Direction ChooseDirection(Rival rival)
    {
        var options = new List<Direction>(4);
        foreach (Direction d in DirectionExtensions.All)
        {
            if (d != rival.Direction.Opposite() && Maze.CanMove(rival.Position, d, allowDoor: false))
                options.Add(d);
        }

        if (options.Count == 0)
        {
            Direction back = rival.Direction.Opposite();
            return Maze.CanMove(rival.Position, back, allowDoor: false) ? back : Direction.None;
        }

        if (rival.Frightened) return options[_random.Next(options.Count)];

        Point target = Mode == RivalMode.Scatter ? rival.ScatterTarget : ChaseTarget(rival);
        Direction best = options[0];
        int bestDistance = int.MaxValue;
        foreach (Direction d in options)
        {
            int distance = Maze.Step(rival.Position, d).DistanceSquared(target);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                best = d;
            }
        }
        return best;
    }

    /// <summary>Cada rival tem um estilo de marcação diferente.</summary>
    public Point ChaseTarget(Rival rival)
    {
        Point player = Player.Position;
        switch (rival.Kind)
        {
            case RivalKind.Flamengo:
                // Marcação individual: vai direto no Palmeiras.
                return player;

            case RivalKind.Corinthians:
                // Tenta antecipar a jogada: mira 4 casas à frente do Palmeiras.
                Point delta = Player.Direction.Delta();
                return new Point(player.X + 4 * delta.X, player.Y + 4 * delta.Y);

            default:
                // São Paulo: persegue de longe, mas recua quando chega perto.
                return rival.Position.DistanceSquared(player) > 36 ? player : rival.ScatterTarget;
        }
    }

    /// <returns>true se o Palmeiras foi pego (o tick deve parar).</returns>
    private bool CheckCollisions()
    {
        foreach (Rival rival in _rivals)
        {
            if (rival.Position != Player.Position) continue;
            if (rival.State is RivalState.Eaten or RivalState.InHouse) continue;

            if (rival.Frightened)
            {
                _rivalsEatenCombo++;
                int points = RivalBasePoints << (_rivalsEatenCombo - 1);
                Score += points;
                rival.State = RivalState.Eaten;
                rival.Frightened = false;
                _popups.Add(new Popup(rival.Position, $"+{points}", PopupTicks));
                ShowBanner($"GOL DO VERDÃO! {rival.Name} derrotado (+{points})");
                continue;
            }

            Lives--;
            State = GameState.Dying;
            StateTicks = DyingTicks;
            FrightenedTicks = 0;
            ShowBanner(Lives > 0 ? $"O {rival.Name} marcou! Restam {Lives} vida(s)" : "FIM DE JOGO...");
            return true;
        }
        return false;
    }
}
