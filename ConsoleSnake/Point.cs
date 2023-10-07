namespace ConsoleSnake;

public class Point
{
    public int Y { get; set; }
    public int X { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Point point)
        {
            return point.X == X && point.Y == Y;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Y, X);
    }

    public static bool operator ==(Point left, Point right) => left.Equals(right);

    public static bool operator !=(Point left, Point right) => !(left == right);
}