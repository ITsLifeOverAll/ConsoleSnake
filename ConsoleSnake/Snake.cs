using ConsoleSnake;

public class Snake
{
    private const int MinSnakeLength = 4;

    private readonly Board _board;
    private readonly LinkedList<Point> _body = new();
    private readonly Point _foodPlace;
    private readonly LinkedList<Direction> _keyList = new();

    public Snake(Board board)
    {
        _board = board;

        for (int i = 0; i < MinSnakeLength; i++) 
            _body.AddLast(new Point(_board.Width / 2 + i, _board.Height / 2));
        
        // draw the snake on console
        foreach (var point in _body)
        {
            Console.SetCursorPosition(point.X, point.Y);
            Console.Write("@");
        }

        _foodPlace = PutFoodRandomly();

        _keyList.AddLast(Direction.Left);

    }

    private Point PutFoodRandomly()
    {
        Point foodPoint;
        do
        {
            foodPoint = new Point(
                new Random().Next(0, _board.Width), 
                new Random().Next(0, _board.Height));
        } while (_body.Contains(foodPoint));
        // draw the food on console
        Console.SetCursorPosition(foodPoint.X, foodPoint.Y);
        Console.Write("X");
        return foodPoint;
    }
}