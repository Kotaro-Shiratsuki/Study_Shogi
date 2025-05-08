using UnityEngine;

/// <summary>
/// Helper class that takes care of the interplay between the array indexes and the Shogi board numbers.
/// </summary>
public class Index
{
    private string[] rows = { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
    private string[] columns = { "一", "二", "三", "四", "五", "六", "七", "八", "九" };

    /// <summary>
    /// Parse grid number to grid number's string
    /// </summary>
    public string ParseShogiPosition(Vector2Int position)
    {
        string row = rows[position.x - 1];
        string column = columns[position.y - 1];

        return GenerateShogiPosition(row, column);
    }

    /// <summary>
    /// Parse two dimentional array index to grid number
    /// </summary>
    public string ParseIndexToShogiPosition(Vector2Int index)
    {
        string row = string.Empty;
        string column = string.Empty;

        row = ParseIndexColumnToShogiRow(index.y);
        column = ParseIndexRowToShogiColumn(index.x);

        return GenerateShogiPosition(row, column);
    }

    /// <summary>
    /// Parse grid number to two dimentional array index
    /// </summary>
    public Vector2Int ParseShogiPositionToIndex(string position)
    {
        Vector2Int index = new Vector2Int();
        string[] tmp = position.Split(',');

        index.x = ParseShogiColumnToIndexRow(tmp[1]);
        index.y = ParseShogiRowToIndexColumn(tmp[0]);

        return index;
    }

    public Vector2Int ParseIndexToShogiPosNum(Vector2Int index)
    {
        Vector2Int pos = new Vector2Int();

        pos.x = ParseIndexColumnToShogiRowNum(index.y);
        pos.y = ParseIndexRowToShogiColumnNum(index.x);

        return pos;
    }

    public bool CheckArrayRange(int indexRow, int indexColumn)
    {
        return (indexRow >= 0 && rows.Length > indexRow) && (indexColumn >= 0 && columns.Length > indexColumn); 
    }

    public string GenerateShogiPosition(string row, string column)
    {
        return row + "," + column;
    }

    private string ParseIndexRowToShogiColumn(int row)
    {
        return columns[row];
    }

    private string ParseIndexColumnToShogiRow(int column)
    {
        int index = rows.Length - column - 1;
        return rows[index];
    }

    private int ParseIndexColumnToShogiRowNum(int column)
    {
        int index = rows.Length - column - 1;
        return StringToInt(rows[index]);
    }

    private int ParseIndexRowToShogiColumnNum(int row)
    {
        return ParseShogiColumnToIndexRow(columns[row]) + 1;
    }

    private int ParseShogiColumnToIndexRow(string column)
    {
        int row = 0;

        for(; row < columns.Length; row++)
        {
            if(columns[row] == column)
            {
                break;
            }
        }

        return row;
    }

    private int ParseShogiRowToIndexColumn(string row)
    {
        return rows.Length - StringToInt(row);
    }

    private int StringToInt(string str)
    {
        if (int.TryParse(str, out int num))
        {
            return num;
        }
        else
        {
            return int.MinValue;
        }
    }
}
