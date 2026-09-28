namespace PacVerdao.Core;

public enum Tile
{
    Empty,
    Wall,
    Door,
    /// <summary>Taça da Libertadores (equivale à "pastilha" do Pac-Man).</summary>
    Taca,
    /// <summary>Power-up: Abel Ferreira entra em campo.</summary>
    Abel,
}

/// <summary>
/// Labirinto em grade. Legenda do texto:
/// '#' parede, '-' porta da casa dos rivais, '.' taça, 'A' Abel Ferreira,
/// 'P' início do Palmeiras, 'G' início de um rival, ' ' vazio.
/// Linhas cujas bordas são livres formam um túnel que atravessa a tela.
/// </summary>
public sealed class Maze
{
    private readonly Tile[,] _tiles;
    private readonly List<Point> _rivalStarts = new();

    public int Width { get; }
    public int Height { get; }
    public Point PlayerStart { get; }
    public IReadOnlyList<Point> RivalStarts => _rivalStarts;

    /// <summary>Casa logo acima da porta: onde os rivais saem para o campo.</summary>
    public Point HouseExit { get; }

    public int TacasRemaining { get; private set; }
    public int TotalTacas { get; }

    private Maze(string[] rows)
    {
        Height = rows.Length;
        Width = rows[0].Length;
        _tiles = new Tile[Width, Height];

        Point? player = null;
        Point? door = null;
        for (int y = 0; y < Height; y++)
        {
            if (rows[y].Length != Width)
                throw new ArgumentException($"Linha {y} do labirinto tem largura {rows[y].Length}, esperado {Width}.");

            for (int x = 0; x < Width; x++)
            {
                char c = rows[y][x];
                Tile tile = c switch
                {
                    '#' => Tile.Wall,
                    '-' => Tile.Door,
                    '.' => Tile.Taca,
                    'A' => Tile.Abel,
                    'P' or 'G' or ' ' => Tile.Empty,
                    _ => throw new ArgumentException($"Caractere inválido '{c}' no labirinto ({x},{y})."),
                };
                _tiles[x, y] = tile;

                if (c == 'P') player = new Point(x, y);
                if (c == 'G') _rivalStarts.Add(new Point(x, y));
                if (c == '-' && door is null) door = new Point(x, y);
                if (tile is Tile.Taca or Tile.Abel) TacasRemaining++;
            }
        }

        PlayerStart = player ?? throw new ArgumentException("Labirinto sem posição inicial do Palmeiras ('P').");
        if (door is null) throw new ArgumentException("Labirinto sem porta ('-') na casa dos rivais.");
        if (_rivalStarts.Count == 0) throw new ArgumentException("Labirinto sem rivais ('G').");
        HouseExit = new Point(door.Value.X, door.Value.Y - 1);
        TotalTacas = TacasRemaining;
    }

    public static Maze Parse(IReadOnlyList<string> rows) => new(rows.ToArray());

    public Tile this[Point p] => InBounds(p) ? _tiles[p.X, p.Y] : Tile.Wall;

    public bool InBounds(Point p) => p.X >= 0 && p.X < Width && p.Y >= 0 && p.Y < Height;

    /// <summary>Dá a volta pelo túnel horizontal.</summary>
    public Point Wrap(Point p) => new(((p.X % Width) + Width) % Width, p.Y);

    public Point Step(Point p, Direction d) => Wrap(p + d.Delta());

    public bool IsWalkable(Point p, bool allowDoor)
    {
        Tile t = this[p];
        return t != Tile.Wall && (allowDoor || t != Tile.Door);
    }

    public bool CanMove(Point from, Direction d, bool allowDoor) =>
        d != Direction.None && IsWalkable(Step(from, d), allowDoor);

    /// <summary>Recolhe o que houver na casa e devolve o que foi recolhido.</summary>
    public Tile Consume(Point p)
    {
        Tile t = this[p];
        if (t is Tile.Taca or Tile.Abel)
        {
            _tiles[p.X, p.Y] = Tile.Empty;
            TacasRemaining--;
            return t;
        }
        return Tile.Empty;
    }

    /// <summary>Primeiro passo do menor caminho (busca em largura) de <paramref name="from"/> até <paramref name="to"/>.</summary>
    public Direction NextStepTowards(Point from, Point to, bool allowDoor)
    {
        if (from == to) return Direction.None;

        var firstStep = new Dictionary<Point, Direction> { [from] = Direction.None };
        var queue = new Queue<Point>();
        queue.Enqueue(from);
        while (queue.Count > 0)
        {
            Point current = queue.Dequeue();
            foreach (Direction d in DirectionExtensions.All)
            {
                Point next = Step(current, d);
                if (firstStep.ContainsKey(next) || !IsWalkable(next, allowDoor)) continue;

                Direction origin = current == from ? d : firstStep[current];
                if (next == to) return origin;
                firstStep[next] = origin;
                queue.Enqueue(next);
            }
        }
        return Direction.None;
    }
}
