namespace ConsoleSnake;

public class Board
{
    public int Width { get; set; }

    public int Height { get; set; }

    public Board()
    {
        Width = Console.WindowWidth;
        Height = Console.WindowHeight;
    }

    public void WriteAt(Point point)
    {
        Console.SetCursorPosition(point.X, point.Y);
        Console.Write("@");
    }
}