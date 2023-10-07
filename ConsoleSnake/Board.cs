public class Board
{
    public int Width { get; set; }

    public int Height { get; set; }

    public Board()
    {
        Width = Console.WindowWidth;
        Height = Console.WindowHeight;
    }

}