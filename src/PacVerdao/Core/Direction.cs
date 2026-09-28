namespace PacVerdao.Core;

public enum Direction
{
    None,
    Up,
    Left,
    Down,
    Right,
}

public readonly record struct Point(int X, int Y)
{
    public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);

    public int DistanceSquared(Point other)
    {
        int dx = X - other.X;
        int dy = Y - other.Y;
        return dx * dx + dy * dy;
    }
}

public static class DirectionExtensions
{
    /// <summary>Ordem de desempate usada pelos rivais (igual ao Pac-Man clássico).</summary>
    public static readonly Direction[] All = { Direction.Up, Direction.Left, Direction.Down, Direction.Right };

    public static Direction Opposite(this Direction d) => d switch
    {
        Direction.Up => Direction.Down,
        Direction.Down => Direction.Up,
        Direction.Left => Direction.Right,
        Direction.Right => Direction.Left,
        _ => Direction.None,
    };

    public static Point Delta(this Direction d) => d switch
    {
        Direction.Up => new Point(0, -1),
        Direction.Down => new Point(0, 1),
        Direction.Left => new Point(-1, 0),
        Direction.Right => new Point(1, 0),
        _ => new Point(0, 0),
    };
}
