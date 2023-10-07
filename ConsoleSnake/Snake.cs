namespace ConsoleSnake;

public class Snake
{
    private const int MinSnakeLength = 4;
    private const int MaxSnakeLength = 6;
    private readonly TimeSpan _moveDuration = TimeSpan.FromMilliseconds(150);

    private readonly Board _board;
    public LinkedList<Point> Body { get; } = new();
    private Point _foodPlace;
    public LinkedList<Direction> KeyList { get; } = new();
    private readonly CancellationTokenSource _cts;

    public Snake(Board board, CancellationTokenSource cts)
    {
        _board = board;
        _cts = cts;

        for (int i = 0; i < MinSnakeLength; i++) 
            Body.AddLast(new Point(_board.Width / 2 + i, _board.Height / 2));
        
        // draw the snake on console
        foreach (var point in Body) 
            _board.WriteAt(point);

        _foodPlace = PutFoodRandomly();

        KeyList.AddLast(Direction.Left);
    }

    private Point PutFoodRandomly()
    {
        Point foodPoint;
        do
        {
            foodPoint = new Point(
                new Random().Next(0, _board.Width), 
                new Random().Next(0, _board.Height));
        } while (Body.Contains(foodPoint));
        // draw the food on console
        Console.SetCursorPosition(foodPoint.X, foodPoint.Y);
        Console.Write("X");
        return foodPoint;
    }

    public void KeyPressed(ConsoleKeyInfo key)
    {
        // only accept arrow keys to _keyList
        if (key.Key == ConsoleKey.UpArrow)
            KeyList.AddLast(Direction.Up);
        else if (key.Key == ConsoleKey.DownArrow)
            KeyList.AddLast(Direction.Down);
        else if (key.Key == ConsoleKey.LeftArrow)
            KeyList.AddLast(Direction.Left);
        else if (key.Key == ConsoleKey.RightArrow)
            KeyList.AddLast(Direction.Right);
    }

    public async Task RunAsync()
    {
        while (true)
        {
            if (_cts.IsCancellationRequested)
            {
                Console.WriteLine("Snake stopped due to cancellation.");
                return;
            }

            var way = Direction.Left;
            if (KeyList.Count > 0)
            {
                way = KeyList.First!.Value;
                KeyList.RemoveFirst();
            }

            var dead = Move(way);
            if (dead)
            {
                Console.WriteLine("Game Over.");
                Environment.Exit(0);
            }

            if (Body.Count >= MaxSnakeLength)
            {
                Console.WriteLine("You Win.");
                Environment.Exit(0);
            }
            
            if (KeyList.Count == 0) KeyList.AddLast(way);
            await Task.Delay(_moveDuration);
        }
    }

    private bool Move(Direction way)
    {
        var headingPoint = way switch
        {
            Direction.Up => new Point(Body.First!.Value.X, Body.First!.Value.Y - 1),
            Direction.Down => new Point(Body.First!.Value.X, Body.First!.Value.Y + 1),
            Direction.Left => new Point(Body.First!.Value.X - 1, Body.First!.Value.Y),
            Direction.Right => new Point(Body.First!.Value.X + 1, Body.First!.Value.Y),
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
        Body.AddFirst(point);
        Console.SetCursorPosition(point.X, point.Y);
        Console.Write("@");

        var last = Body.Last!.Value;
        Body.RemoveLast();
        Console.SetCursorPosition(last.X, last.Y);
        Console.Write(" ");
    }

    private bool EatFood(Point point)
    {
        if (point != _foodPlace) return false;

        Body.AddFirst(point);
        Console.SetCursorPosition(point.X, point.Y);
        Console.Write("@");

        _foodPlace = PutFoodRandomly();
        return true;
    }

    private bool EatSelf(Point point) => Body.Contains(point);

    private bool HitWall(Point point)
    {
        // if the snake hits the wall, it dies, return true
        if (point.X < 0 || point.X >= _board.Width 
                        || point.Y < 0 || point.Y >= _board.Height)
            return true;

        return false;
    }
}