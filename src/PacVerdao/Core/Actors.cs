namespace PacVerdao.Core;

/// <summary>O Palmeiras (o "Pac-Man" do jogo).</summary>
public sealed class Player
{
    public Point Position { get; set; }
    public Direction Direction { get; set; } = Direction.Left;

    /// <summary>Direção pedida pelo jogador; aplicada assim que o caminho estiver livre.</summary>
    public Direction NextDirection { get; set; } = Direction.None;

    public bool IsMoving { get; set; }
    internal int MoveCounter { get; set; }
}

public enum RivalKind
{
    Flamengo,
    Corinthians,
    SaoPaulo,
}

public enum RivalState
{
    /// <summary>Esperando na casa (vestiário) para entrar em campo.</summary>
    InHouse,
    /// <summary>Saindo da casa pela porta.</summary>
    Leaving,
    /// <summary>Em campo, caçando o Palmeiras.</summary>
    Active,
    /// <summary>Derrotado pelo Palmeiras com o Abel; volta para a casa.</summary>
    Eaten,
}

/// <summary>Um time rival (o "fantasma" do Pac-Man).</summary>
public sealed class Rival
{
    public Rival(RivalKind kind, Point home, Point scatterTarget, int releaseIndex)
    {
        Kind = kind;
        Home = home;
        ScatterTarget = scatterTarget;
        ReleaseIndex = releaseIndex;
        Position = home;
    }

    public RivalKind Kind { get; }
    public Point Home { get; }
    public Point ScatterTarget { get; }
    public int ReleaseIndex { get; }

    public Point Position { get; set; }
    public Direction Direction { get; set; } = Direction.Up;
    public RivalState State { get; set; } = RivalState.InHouse;

    /// <summary>Assustado: o Abel Ferreira está em campo e o rival pode ser derrotado.</summary>
    public bool Frightened { get; set; }

    internal bool ReverseRequested { get; set; }
    internal int MoveCounter { get; set; }

    public string Name => Kind switch
    {
        RivalKind.Flamengo => "Flamengo",
        RivalKind.Corinthians => "Corinthians",
        RivalKind.SaoPaulo => "São Paulo",
        _ => Kind.ToString(),
    };

    public string Tag => Kind switch
    {
        RivalKind.Flamengo => "FL",
        RivalKind.Corinthians => "CO",
        RivalKind.SaoPaulo => "SP",
        _ => "??",
    };
}

/// <summary>Texto flutuante no campo (ex.: "+400" ao derrotar um rival).</summary>
public sealed class Popup
{
    public Popup(Point position, string text, int ticks)
    {
        Position = position;
        Text = text;
        TicksLeft = ticks;
    }

    public Point Position { get; }
    public string Text { get; }
    public int TicksLeft { get; set; }
}
