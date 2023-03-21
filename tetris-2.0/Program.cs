using tetris_2._0;

var rows = 16;
var columns = 10;
var game = new Game(rows,columns);
Console.CursorVisible = false;
game.Start();


while (!game.GameOver)
{
    // listen to key presses
    if (Console.KeyAvailable)
    {
        var input = Console.ReadKey(true);
        switch (input.Key)
        {
            // send key presses to the game if it's not paused
            case ConsoleKey.UpArrow:
            case ConsoleKey.DownArrow:
            case ConsoleKey.LeftArrow:
            case ConsoleKey.RightArrow:
                if (!game.Paused)
                    game.Input(input.Key);
                break;

            case ConsoleKey.P:
                if (game.Paused)
                    game.Resume();
                else
                    game.Pause();
                break;

            case ConsoleKey.Escape:
                game.Stop();
                return;
        }
    }
}

