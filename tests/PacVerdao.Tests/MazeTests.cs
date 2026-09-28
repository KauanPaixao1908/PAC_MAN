using PacVerdao.Core;
using Xunit;

namespace PacVerdao.Tests;

public class MazeTests
{
    public static IEnumerable<object[]> AllPhases() =>
        Phases.All.Select((phase, index) => new object[] { index });

    [Theory]
    [MemberData(nameof(AllPhases))]
    public void Every_taca_is_reachable_from_the_start(int phaseIndex)
    {
        Maze maze = Maze.Parse(Phases.All[phaseIndex].Layout);
        HashSet<Point> reachable = Flood(maze, maze.PlayerStart, allowDoor: false);

        for (int y = 0; y < maze.Height; y++)
        for (int x = 0; x < maze.Width; x++)
        {
            var p = new Point(x, y);
            if (maze[p] is Tile.Taca or Tile.Abel)
                Assert.Contains(p, reachable);
        }
    }

    [Theory]
    [MemberData(nameof(AllPhases))]
    public void Field_has_no_dead_ends(int phaseIndex)
    {
        Maze maze = Maze.Parse(Phases.All[phaseIndex].Layout);
        foreach (Point p in Flood(maze, maze.PlayerStart, allowDoor: false))
        {
            int exits = DirectionExtensions.All.Count(d => maze.CanMove(p, d, allowDoor: false));
            Assert.True(exits >= 2, $"Beco sem saída em {p}");
        }
    }

    [Theory]
    [MemberData(nameof(AllPhases))]
    public void Rivals_can_leave_the_house_and_player_cannot_enter(int phaseIndex)
    {
        Maze maze = Maze.Parse(Phases.All[phaseIndex].Layout);
        Assert.Equal(3, maze.RivalStarts.Count);
        foreach (Point start in maze.RivalStarts)
        {
            Assert.DoesNotContain(start, Flood(maze, maze.PlayerStart, allowDoor: false));
            Assert.Contains(maze.HouseExit, Flood(maze, start, allowDoor: true));
        }
    }

    [Fact]
    public void Tunnel_wraps_around_the_screen()
    {
        Maze maze = Maze.Parse(Layouts.Classico);
        var leftEdge = new Point(0, 9);
        Assert.Equal(new Point(maze.Width - 1, 9), maze.Step(leftEdge, Direction.Left));
        Assert.True(maze.CanMove(leftEdge, Direction.Left, allowDoor: false));
    }

    [Fact]
    public void Shortest_path_finds_the_house_exit()
    {
        Maze maze = Maze.Parse(Layouts.Classico);
        Assert.Equal(Direction.Up, maze.NextStepTowards(new Point(13, 9), maze.HouseExit, allowDoor: true));
        Assert.Equal(Direction.None, maze.NextStepTowards(new Point(13, 9), maze.HouseExit, allowDoor: false));
    }

    private static HashSet<Point> Flood(Maze maze, Point start, bool allowDoor)
    {
        var seen = new HashSet<Point> { start };
        var queue = new Queue<Point>(new[] { start });
        while (queue.Count > 0)
        {
            Point p = queue.Dequeue();
            foreach (Direction d in DirectionExtensions.All)
            {
                Point next = maze.Step(p, d);
                if (maze.IsWalkable(next, allowDoor) && seen.Add(next)) queue.Enqueue(next);
            }
        }
        return seen;
    }
}
