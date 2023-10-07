// See https://aka.ms/new-console-template for more information

Console.CursorVisible = false;
Console.Clear();

var cts = new CancellationTokenSource();
var board = new Board(); 
var snake = new Snake(board, cts);


#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
snake.RunAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed


while (true)
{
    var key = Console.ReadKey(true);
    if (IsEscapeKey(key))
    {
        cts.Cancel();
        await Task.Delay(1000);
        break;
    }

    snake.KeyPressed(key);
}

return;

static bool IsEscapeKey(ConsoleKeyInfo key) => key.Key == ConsoleKey.Escape;