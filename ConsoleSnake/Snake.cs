using ConsoleSnake;

public class Snake
{
    private const int MinSnakeLength = 4;
    private const int MaxSnakeLength = 6;

    private readonly Board _board;
    private readonly LinkedList<Point> _body = new();
    private Point _foodPlace;
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

    public void KeyPressed(ConsoleKeyInfo key)
    {
        // only accept arrow keys to _keyList
        if (key.Key == ConsoleKey.UpArrow)
            _keyList.AddLast(Direction.Up);
        else if (key.Key == ConsoleKey.DownArrow)
            _keyList.AddLast(Direction.Down);
        else if (key.Key == ConsoleKey.LeftArrow)
            _keyList.AddLast(Direction.Left);
        else if (key.Key == ConsoleKey.RightArrow)
            _keyList.AddLast(Direction.Right);

    }

    public void RunAsync()
    {
        var way = Direction.Left;
        if (_keyList.Count > 0)
        {
            way = _keyList.First!.Value;
            _keyList.RemoveFirst();
        }

        var dead = Move(way);
        if (dead)
        {
            Console.WriteLine("Game Over.");
            Environment.Exit(0);
        }

        if (_body.Count >= MaxSnakeLength)
        {
            Console.WriteLine("You Win.");
            Environment.Exit(0);
        }

    }

    private bool Move(Direction way)
    {
        var headingPoint = way switch
        {
            Direction.Up => new Point(_body.First!.Value.X, _body.First!.Value.Y - 1),
            Direction.Down => new Point(_body.First!.Value.X, _body.First!.Value.Y + 1),
            Direction.Left => new Point(_body.First!.Value.X - 1, _body.First!.Value.Y),
            Direction.Right => new Point(_body.First!.Value.X + 1, _body.First!.Value.Y),
            _ => throw new ArgumentOutOfRangeException(nameof(way), way, null)
        };
        return MoveResult(headingPoint);

    }

    private bool MoveResult(Point point)
    {
        if (HitWall(point)) return true;
        if (EatSelf(point)) return true;
        if (EatFood(point)) return false;
        MoveSafely(point);
        return false;
    }

    private void MoveSafely(Point point)
    {
        _body.AddFirst(point);
        Console.SetCursorPosition(point.X, point.Y);
        Console.Write("@");

        var last = _body.Last!.Value;
        _body.RemoveLast();
        Console.SetCursorPosition(last.X, last.Y);
        Console.Write(" ");
    }

    private bool EatFood(Point point)
    {
        if (point.X == _foodPlace.X && point.Y == _foodPlace.Y)
        {
            _body.AddFirst(point);
            Console.SetCursorPosition(point.X, point.Y);
            Console.Write("@");

            _foodPlace = PutFoodRandomly();
            return true;
        }
        return false;
    }

    private bool EatSelf(Point point) => _body.Contains(point);

    private bool HitWall(Point point)
    {
        // if the snake hits the wall, it dies, return true
        if (point.X < 0 || point.X >= _board.Width 
                        || point.Y < 0 || point.Y >= _board.Height)
            return true;
        return false;
    }
}