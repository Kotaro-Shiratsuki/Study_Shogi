using System.Collections.Generic;
using System;
using UnityEngine;

public class GridStatusController
{
    private GameManager manager;
    private Index index;
    private MovementDataParser parser;
    private KomaDictionaryController komaDic;
    private GridDictionaryController gridDic;
    private LimitedKomaRowPairs limit;
    private int[,] movementArea;

    public GridStatusController(GameManager manager)
    {
        this.manager = manager;
        index = manager.indexController;
        parser = manager.movementDataParser;
        komaDic = manager.KomaDictionary;
        gridDic = manager.GridDictionary;
        limit = new LimitedKomaRowPairs();
        movementArea = new int[9, 9];
    }

    internal void UpdateMovableGrid(Vector2Int clicked)
    {
        string key = index.ParseShogiPosition(clicked);
        // Check : Is there any koma in clickedposition
        if (komaDic.TryGetValue(key, out var koma))
        {
            // Clear movementArea
            Array.Clear(movementArea, 0, movementArea.Length);

            List<MovesListRow> targetMoves = koma.GetIsEvolved() ? koma.Data.MovementData.ExtraMoving : koma.Data.MovementData.DefaultMoving;
            movementArea = parser.GetMovementArea(targetMoves, key, IsReverse(koma));

            string debuglog = "Movement area map :\n";
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    debuglog += movementArea[i, j].ToString() + ", ";
                }

                debuglog += "\n";
            }
            Debug.Log(debuglog);

            ChangeGridStateToMovable(key, koma.GetCanEvolution());
        }
        else
        {
            return;
        }
    }

    internal void UpdateMovableGridOnSide(ID id, bool isFriend)
    {
        // Clear movementArea
        Array.Clear(movementArea, 0, movementArea.Length);

        GetMovementAreaOnSide(id, isFriend);
    }

    private void ChangeGridStateToMovable(string key, bool canEvolution)
    {
        int startX = komaDic.GetPairKey(key).x;
        int startY = komaDic.GetPairKey(key).y;

        // Searching directions
        Vector2Int[] directions = new Vector2Int[]
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
            new Vector2Int(2, -1),  // Jump to lower left
            new Vector2Int(2, 1),   // Jump to lower right
        };

        SearchGridInDirection(directions, startX, startY, canEvolution);
    }

    private void SearchGridInDirection(Vector2Int[] directions, int startX, int startY, bool canEvolution)
    {
        Vector2Int startPosition = new Vector2Int(startX, startY);

        foreach (Vector2Int dir in directions)
        {
            int currentRow = startX;
            int currentColumn = startY;

            Vector2Int nextTarget = Vector2Int.zero;

            while (true)
            {
                int nextRow = currentRow + dir.x;
                int nextColumn = currentColumn + dir.y;

                nextTarget = new Vector2Int(nextRow, nextColumn);

                if (!manager.CheckArrayRange(nextRow, nextColumn, movementArea.GetLength(0), movementArea.GetLength(1)))
                {
                    break;
                }

                if (movementArea[nextRow, nextColumn] != 1)
                {
                    break;
                }

                if (gridDic.GetGridState(nextTarget) == GridState.Empty)
                {
                    gridDic.ChangeGridState(nextTarget, GetWitchMovableArea(startPosition, nextRow, canEvolution));
                }
                else
                {
                    gridDic.ChangeGridState(nextTarget, GetAttackableOrBlockedArea(startPosition, nextTarget, canEvolution));
                    break;
                }

                currentRow = nextRow;
                currentColumn = nextColumn;
            }
        }
    }

    private GridState GetWitchMovableArea(Vector2Int startPos, int currentRow, bool canEvolution)
    {
        Koma koma = komaDic.GetValue(startPos);

        switch (koma.Owner)
        {
            case Owner.Player:
                if ((startPos.x < 3 || currentRow < 3) && canEvolution)
                {
                    return GridState.MovableWithEv;
                }
                else
                {
                    return GridState.Movable;
                }

            case Owner.Enemy:
                if ((startPos.x > 5 || currentRow > 5) && canEvolution)
                {
                    return GridState.MovableWithEv;
                }
                else
                {
                    return GridState.Movable;
                }

            default:
                break;
        }

        return GridState.None;
    }

    private GridState GetAttackableOrBlockedArea(Vector2Int position, Vector2Int target, bool canEvolution)
    {
        if (komaDic.TryGetValue(position, out var koma))
        {
            switch (koma.Owner)
            {
                case Owner.Player:
                    if (gridDic.GetValue(target).GridData.State == GridState.Enemy)
                    {
                        if ((position.x < 3 || target.x < 3) && canEvolution)
                        {
                            return GridState.AttackableWithEv;
                        }
                        else
                        {
                            return GridState.Attackable;
                        }
                    }
                    else
                    {
                        return GridState.Blocked;
                    }

                case Owner.Enemy:
                    if (gridDic.GetValue(target).GridData.State == GridState.Friend)
                    {
                        if ((position.x > 5 || target.x > 5) && canEvolution)
                        {
                            return GridState.AttackableWithEv;
                        }
                        else
                        {
                            return GridState.Attackable;
                        }
                    }
                    else
                    {
                        return GridState.Blocked;
                    }

                default:
                    break;
            }
        }

        return GridState.None;
    }

    private void GetMovementAreaOnSide(ID id, bool isFriend)
    {
        List<int> limitRow = limit.GetLimitedRow(id, isFriend);

        foreach (var grid in gridDic.GetAllValues())
        {
            Vector2Int index = grid.GridData.IndexNumber;

            if (limitRow.Contains(index.x))
            {
                continue;
            }
            else
            {
                if (grid.GridData.State == GridState.Empty)
                {
                    grid.ChangeState(GridState.Movable);
                }
            }
        }
    }

    private bool IsReverse(Koma clicked)
    {
        return clicked.Owner == Owner.Enemy;
    }
}