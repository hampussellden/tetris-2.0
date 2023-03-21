using System.Data;

namespace tetris_2._0;

public class GameGrid
{
    public int Columns { get; }
    public int Rows { get; }
    public int[,] Grid;

    public char[] TileCharacters =
    {
        '-', 'I', 'J', 'L', 'O', 'S', 'T', 'Z'
    };

    public ConsoleColor[] TileColors =
    {
        ConsoleColor.Black,
        ConsoleColor.Cyan,
        ConsoleColor.Blue,
        ConsoleColor.DarkYellow,
        ConsoleColor.Yellow,
        ConsoleColor.Green,
        ConsoleColor.Magenta,
        ConsoleColor.Red
    };

    public GameGrid(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        Grid = new int[rows, columns];
        for (int r = 0;  r< Rows; r++)
        {
            for (int c = 0; c < Columns; c++)
            {
                Grid[r, c] = 0;
            }
        }
    }

    public bool IsInside(int r, int c)
    {
        return r >= 0 && r < Rows && c >= 0 && c < Columns;
    }

    public bool IsEmpty(int r, int c)
    {
        return IsInside(r, c) && Grid[r, c] == 0;
    }

    public bool RowIsFull(int r)
    {
        for (int c = 0; c < Columns; c++)
        {
            if (Grid[r, c] == 0)
            {
                return false; //returns false as soon as we find an empty cell
            }
        }

        return true;
    }
    
    public bool RowIsEmpty(int r)
    {
        for (int c = 0; c < Columns; c++)
        {
            if (Grid[r, c] != 0)
            {
                return false; 
            }
        }

        return true;
    }
    public void ClearRow(int r)
    {
        for (int c = 0; c < Columns; c++)
        {
            Grid[r, c] = 0;
        }
    }
    public void MoveRowDown(int r, int count)
    {
        for (int c = 0; c < Columns; c++)
        {
            Grid[r + count, c] = Grid[r, c];
            Grid[r, c] = 0;
        }
    }
    public int ClearFullRows()
    {
        int count = 0;
        for (int r = Rows - 1; r >= 0; r--)
        {
            if (RowIsFull(r))
            {
                ClearRow(r);
                count++;
            } else if (count > 0)
            {
                MoveRowDown(r,count);
            }
        }
        return count;
    }
}