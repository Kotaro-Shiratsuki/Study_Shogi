using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoSingleton<GameManager>
{
    public enum GameState
    {
        None,
        Boot,
        Title,
        Home,
        Game,
        Loading,
    }

    [field: Header("Game State")]
    [field: SerializeField]
    public GameState CurrentState { get; private set; } = GameState.Game; // 一旦仮でGameで初期化

    [Header("Dictionaries")]
    public DualKeyDictionary<Vector2Int, Vector2Int, ShogiGrid> GridDictionary;
    public DualKeyDictionary<Vector2Int, Vector2Int, Koma> KomaDictionary;

    private MovementDataParser movementDataParser;
    private int[,] movementArea;

    protected override void Awake()
    {
        base.Awake();

        movementDataParser = new MovementDataParser();
        movementArea = new int[9, 9];
    }


    public void UpdateOnMoved(Vector2Int prePosition, Vector2Int postPosition)
    {
        UpdateKomaDictionary(prePosition, postPosition);
        UpdateGridDictionaryOnMoved(prePosition, postPosition);
    }

    /// <summary>
    /// Update koma dictionary afetr move.
    /// </summary>
    public void UpdateKomaDictionary(Vector2Int prePosition, Vector2Int postPosition)
    {
        Koma koma;
        if(KomaDictionary.TryGetValueFirstKey(prePosition, out koma))
        {
            KomaDictionary.Remove(prePosition);
            KomaDictionary.Add(postPosition, ParseShogiPosToTwoDimentionalIndex(postPosition), koma);
        }
    }

    public void UpdateGridDictionaryOnMoved(Vector2Int prePosition, Vector2Int postPosition)
    {
        GridDictionary.GetValueByFirstKey(prePosition).GridData.SetState(GridState.Empty);
        GridDictionary.GetValueByFirstKey(postPosition).GridData.SetState(GridState.Ruled);
    }

    /// <summary>
    /// Get koma's movement area in clicked position.
    /// And update empty grid to movable.
    /// </summary>
    public void UpdateMovableGrid(Vector2Int clicked)
    {
        // Check : Is there any koma in clickedposition
        if(KomaDictionary.TryGetValueFirstKey(clicked, out var koma))
        {
            // Clear movementArea
            Array.Clear(movementArea, 0, movementArea.Length);

            // Get koma's moavement areas. Ignore 'Nari' and 'Owner' in instant.
            movementArea = movementDataParser.GetMovementArea(koma.Data.MovementData.DefaultMoving, clicked, IsReverse(clicked));

            string debuglog = "Movement area map :\n";
            for (int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    debuglog += movementArea[i,j].ToString() + ", ";
                }

                debuglog += "\n";
            }
            Debug.Log(debuglog);

            ChangeGridStateToMovable(clicked);
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// Remove 'Movable' state from movablegrid.
    /// </summary>
    public void ResetMovableGrid()
    {
        var grids = GridDictionary.GetAllValue();
        foreach (var grid in grids)
        {
            if(grid.GridData.State == GridState.Movable)
            {
                grid.GridData.SetState(GridState.Empty);
            }
        }
    }

    /// <summary>
    /// Check is position out of range.
    /// </summary>
    public bool CheckArrayRange(int x, int y, int rows, int cols)
    {
        return x >= 0 && x < rows && y >= 0 && y < cols;
    }

    public Vector2Int ParseTwoDimentionalIndexToShogiPos(Vector2Int index)
    {
        int parsedX = StaticMembers.BoardColumn - index.y;
        int parsedY = index.x + 1;

        return new Vector2Int(parsedX, parsedY);
    }

    /// <summary>
    /// Parse grid number to two dimentional array position
    /// </summary>
    public Vector2Int ParseShogiPosToTwoDimentionalIndex(Vector2Int position)
    {
        int parsedX = position.y - 1;
        int parsedY;

        switch (position.x)
        {
            case 9:
                parsedY = 0;
                break;

            case 8:
                parsedY = 1;
                break;

            case 7:
                parsedY = 2;
                break;

            case 6:
                parsedY = 3;
                break;

            case 5:
                parsedY = 4;
                break;

            case 4:
                parsedY = 5;
                break;

            case 3:
                parsedY = 6;
                break;

            case 2:
                parsedY = 7;
                break;

            case 1:
                parsedY = 8;
                break;

            default:
                parsedY = -1;
                break;
        }

        return new Vector2Int(parsedX, parsedY);
    }

    #region Private Methods
    private void ChangeGridStateToMovable(Vector2Int clicked)
    {
        int ownX = KomaDictionary.GetPairKeyWithFirst(clicked).x;
        int ownY = KomaDictionary.GetPairKeyWithFirst(clicked).y;

        // Searching directions
        Vector2Int[] direction = new Vector2Int[]
        {
            new Vector2Int(-1, 0),  // Up
            new Vector2Int(1, 0),   // Down
            new Vector2Int(0, -1),  // Left
            new Vector2Int(0, 1),   // Right
            new Vector2Int(-1, -1), // Upper left
            new Vector2Int(-1, 1),  // Upper right
            new Vector2Int(1, -1),  // Lower left
            new Vector2Int(1, 1),   // Lower right
            new Vector2Int(-2, -1), // Jump to upper left
            new Vector2Int(-2, 1),  // Jump to upper right

        };

        foreach (Vector2Int dir in direction)
        {
            int currentRow = ownX;
            int currentColumn = ownY;

            while (true)
            {
                int nextRow = currentRow + dir.x;
                int nextColumn = currentColumn + dir.y;

                if (!CheckArrayRange(nextRow, nextColumn, movementArea.GetLength(0), movementArea.GetLength(1)))
                {
                    break;
                }

                if (movementArea[nextRow, nextColumn] != 1)
                {
                    break;
                }

                if (GridDictionary.TryGetValueSecondKey(new Vector2Int(nextRow, nextColumn), out var grid))
                {
                    if (grid.GridData.State == GridState.Empty)
                    {
                        grid.GridData.SetState(GridState.Movable);
                    }
                    else if (grid.GridData.State == GridState.Ruled)
                    {
                        //grid.GridData.SetState(GridState.Movable);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }

                currentRow = nextRow;
                currentColumn = nextColumn;
            }
        }
    }

    private bool IsReverse(Vector2Int clicked)
    {
        return KomaDictionary.GetValueByFirstKey(clicked).Owner == Owner.Enemy;
    }
    #endregion
}
