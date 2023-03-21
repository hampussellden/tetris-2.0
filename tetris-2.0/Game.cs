namespace tetris_2._0;

//
// This is the game event logic that you can customize and cannibalize
// as needed. You should try to write your game in a modular way, avoid
// making one huge Game class.
//

class Game
{
    ScheduleTimer? _timer;
    public bool Paused { get; private set; }
    private int Score { get; set; }
    private int Level { get; set; }
    private int ClearedRows { get; set; }
    private int GameSpeed { get; set; }
    public bool GameOver { get; private set; }

    public readonly GameGrid Grid;
    public readonly BlockQueue BlockQueue;
    public Game(int rows,int columns)
    {
        Level = 1;
        GameSpeed = 700;
        Grid = new GameGrid(rows, columns);
        BlockQueue = new BlockQueue();
        _currentBlock = BlockQueue.GetAndUpdate();
    }
    private Block _currentBlock;
    public Block CurrentBlock
    {
        get => _currentBlock;
        private set
        {
            _currentBlock = value;
            _currentBlock.Reset();
        }
    }
    public void Start()
    {
        Console.WriteLine("Start");
        ScheduleNextTick();
    }

    public void Pause()
    {
        Paused = true;
        _timer!.Pause();
        Draw();
    }

    public void Resume()
    {
        Paused = false;
        _timer!.Resume();
        Draw();
    }

    public void Stop()
    {
        GameOver = true;
    }

    public void Input(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.RightArrow:
                MoveBlockRight();
                break;
            case ConsoleKey.LeftArrow:
                MoveBlockLeft();
                break;
            case ConsoleKey.UpArrow:
                RotateRight();
                break;
            case ConsoleKey.Z:
                RotateLeft();
                break;
            case ConsoleKey.DownArrow:
                MoveBlockDown();
                break;
            case ConsoleKey.P:
                Pause();
                break;
        }
    }

    private void Tick()
    {
        MoveBlockDown();
        Draw();
        ScheduleNextTick();
    }

    private void ScheduleNextTick()
    {
        // the game will automatically update itself every half a second, adjust as needed
        _timer = new ScheduleTimer(GameSpeed, Tick);
    }

    private bool BlockFits()
    {
        foreach (var p in CurrentBlock.TilePositions())
        {
            if (!Grid.IsEmpty(p.Row, p.Column))
            {
                return false;
            }
        }
        return true;
    }

    private void RotateRight()
    {
        CurrentBlock.RotateRight();
        if (!BlockFits())
        {
            CurrentBlock.RotateLeft();
        }
    }

    private void RotateLeft()
    {
        CurrentBlock.RotateLeft();
        if (!BlockFits())
        {
            CurrentBlock.RotateRight();
        }
    }

    private void MoveBlockRight()
    {
        CurrentBlock.Move(0,1);
        if (!BlockFits())
        {
            CurrentBlock.Move(0,-1);
        }
    }

    private void MoveBlockLeft()
    {
        CurrentBlock.Move(0,-1);
        if (!BlockFits())
        {
            CurrentBlock.Move(0,1);
        }
    }

    private bool IsGameOver()
    {
        return !(Grid.RowIsEmpty(0) && Grid.RowIsEmpty(1));
    }

    private void PlaceBlock()
    {
        foreach (var p in CurrentBlock.TilePositions())
        {
            Grid.Grid[p.Row, p.Column] = CurrentBlock.Id;
        }

        if (IsGameOver())
        {
            GameOver = true;
        }
        else
        {
            CurrentBlock = BlockQueue.GetAndUpdate();
            CurrentBlock.Reset();
        }

        var rowsCleared = Grid.ClearFullRows();
        Score += CalcScore(rowsCleared);
        ClearedRows += rowsCleared;
        SetLevel();
        SetGameSpeed();
    }

    private void MoveBlockDown()
    {
        CurrentBlock.Move(1,0);
        if (!BlockFits())
        {
            CurrentBlock.Move(-1,0);
            PlaceBlock();
        }
    }

    private void Draw()
    {
        Console.Clear();
        DrawGrid(Grid);
        Console.ForegroundColor = Grid.TileColors[0];
        Console.WriteLine("Score: " + Score);
        Console.WriteLine("Level: " + Level);
        Console.WriteLine("Cleared Rows: " + ClearedRows);
        if (Paused)
        {
            Console.WriteLine("Paused");
        }
        if (GameOver)
        {
            Console.WriteLine("Game Over");   
        }
        DrawCurrentBlock(CurrentBlock);
        
    }

    private void DrawGrid(GameGrid grid)
    {
        
        for (var r = 0; r < grid.Rows; r++)
        {
            for (int c = 0; c < grid.Columns; c++)
            {
                var tile = grid.Grid[r, c];
                Console.ForegroundColor = grid.TileColors[tile];
                Console.Write(grid.TileCharacters[tile]);
            }
            Console.WriteLine(" ");
        }
    }

    private void DrawCurrentBlock(Block currentBlock)
    {
        
        foreach (var p in currentBlock.TilePositions())
        {
            Console.SetCursorPosition(p.Column,p.Row);
            var blockId = currentBlock.Id;
            Console.ForegroundColor = Grid.TileColors[blockId];
            Console.Write(Grid.TileCharacters[blockId]);
        }
    }

    private int CalcScore(int rows)
    {
        int score = 0;
        switch (rows)
        {
            case 1:
                score = 100;
            break;
            case 2:
                score = 300;
            break;
            case 3:
                score= 300;
            break;
            case 4:
                score= 300;
                break;
        }
        return score * Level;
    }

    private void SetLevel()
    {
        switch (ClearedRows)
        {
            case 5:
                Level = 2;
                break;
            case 10:
                Level = 3;
                break;
            case 15:
                Level = 4;
                break;
            case 20:
                Level = 5;
                break;
            case 25:
                Level = 6;
                break;
            case 30:
                Level = 7;
                break;
            case 35:
                Level = 8;
                break;
            case 40:
                Level = 9;
                break;
            case 50:
                Level = 10;
                break;

        }
    }
    //will give a range of 200 - 700 dependant on level
    private void SetGameSpeed()
    {
        var a = 700.0;
        var k = (1.0 / 10.0) * Math.Log(a / 200.0);
        GameSpeed = (int)Math.Round(a * Math.Exp(-k * Level));
    }
}