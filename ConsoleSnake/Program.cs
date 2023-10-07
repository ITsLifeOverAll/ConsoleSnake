// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var board = new Board(); 
var snake = new Snake(board);

snake.RunAsync();

while (true)
{
    var key = Console.ReadKey(true);
    if (IsEscapeKey(key))
        return;


    snake.KeyPressed(key);
}

return;

static bool IsEscapeKey(ConsoleKeyInfo key) => key.Key == ConsoleKey.Escape;