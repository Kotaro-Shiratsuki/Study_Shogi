using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parser class for replacing komaMovementData with movement on the board.
/// Returns 9*9 array with movement range as 1.
/// </summary>
public class MovementDataParser
{
    readonly int none = StaticMembers.None;
    readonly int walk = StaticMembers.Walk;
    readonly int fly = StaticMembers.Fly;
    readonly int jump = StaticMembers.Jump;

    readonly int directionColumn = 3;
    readonly int directionRow = 3;

    int[,] movementAreas = new int[StaticMembers.BoardColumn, StaticMembers.BoardRow];

    /// <summary>
    /// Process to return candidate grid as a 9*9 array.
    /// </summary>
    public int[,] GetMovementArea(List<MovesListRow> moves, string key, bool isReverse = false)
    {
        // 0 clear.
        System.Array.Clear(movementAreas, 0, movementAreas.Length);
        int[,] moveDirection = new int[directionColumn, directionRow];

        // Convert the list of MoveType set in MovementData to int.
        moveDirection = ParseMovetypeToDirection(moves);

        // Create a move range array centered on itself for each converted MoveType
        ParseMoveDirectionToMovableAreas(key, moveDirection, isReverse);

        return movementAreas;
    }

    /// <summary>
    /// Process to convert the MoveType to int.
    /// </summary>
    private int[,] ParseMovetypeToDirection(List<MovesListRow> moves)
    {
        int[,] direction = new int[directionColumn, directionRow];

        for (int i = 0; i < moves.Count; i++)
        {
            for(int j = 0; j < moves[i].MovingListRow.Count; j++)
            {
                switch(moves[i].MovingListRow[j])
                {
                    case Movingtype.None:
                        direction[i, j] = none;
                        break;

                    case Movingtype.Walk:
                        direction[i, j] = walk;
                        break;

                    case Movingtype.Fly:
                        direction[i, j] = fly;
                        break;

                    case Movingtype.Jump:
                        direction[i, j] = jump;
                        break;

                    default:
                        direction[i, j] = int.MinValue;
                        break;
                }
            }
        }

        return direction;
    }

    /// <summary>
    /// Assigns a move range centered on itself based on the converted MoveDirection.
    /// </summary>
    private void ParseMoveDirectionToMovableAreas(string key, int[,] direction, bool isReverse)
    {
        Vector2Int center = GameManager.Instance.ParsePosToIndex(key);

        for(int i = 0; i < directionColumn; i++)
        {
            for(int j = 0;j < directionRow; j++)
            {
                if (direction[i,j] == none)
                {
                    continue;
                }
                else if(direction[i,j] == walk)
                {
                    SetWalkAreas(center, ConvertWalkAndFlyDirection(i, j), isReverse);
                }
                else if (direction[i, j] == fly)
                {
                    SetFlyAreas(center, ConvertWalkAndFlyDirection(i, j), isReverse);
                }
                else
                {
                    SetJumpAreas(center, ConvertJumpDirection(i, j), isReverse);
                }
            }
        }
    }

    /// <summary>
    /// MoveDirection == walk
    /// </summary>
    private void SetWalkAreas(Vector2Int center, Vector2Int direction, bool isReverse)
    {
        if(direction == Vector2Int.zero)
        {
            return;
        }

        int forward = isReverse ? -1 : 1;
        int targetX = center.x + direction.x * forward;
        int targetY = center.y + direction.y * forward;

        if (GameManager.Instance.CheckArrayRange(targetX, targetY, movementAreas.GetLength(0), movementAreas.GetLength(1)))
        {
            movementAreas[targetX, targetY] = 1;
        }
    }

    /// <summary>
    /// MoveDirection == fly
    /// </summary>
    private void SetFlyAreas(Vector2Int center, Vector2Int direction, bool isReverse)
    {
        if (direction == Vector2Int.zero)
        {
            return;
        }

        int forward = isReverse ? -1 : 1;

        for (int i = 1; i < StaticMembers.BoardColumn; i++)
        {
            int targetX = center.x + (direction.x * i) * forward;
            int targetY = center.y + (direction.y * i) * forward;

            if (GameManager.Instance.CheckArrayRange(targetX, targetY, movementAreas.GetLength(0), movementAreas.GetLength(1)))
            {
                movementAreas[targetX, targetY] = 1;
            }
            else
            {
                break;
            }
        }
    }

    /// <summary>
    /// MoveDirection == jump
    /// </summary>
    private void SetJumpAreas(Vector2Int center, Vector2Int direction, bool isReverse)
    {
        if (direction == Vector2Int.zero)
        {
            return;
        }

        int forward = isReverse ? -1 : 1;
        int targetX = center.x + direction.x * forward;
        int targetY = center.y + direction.y * forward;

        if (GameManager.Instance.CheckArrayRange(targetX, targetY, movementAreas.GetLength(0), movementAreas.GetLength(1)))
        {
            movementAreas[targetX, targetY] = 1;
        }
    }

    /// <summary>
    /// Process to reconvert the converted direction to a value that can be directly added to or subtracted from the index.
    /// For walk and fly.
    /// </summary>
    private Vector2Int ConvertWalkAndFlyDirection(int directionX, int directionY)
    {
        int convertedX, convertedY;

        switch (directionX)
        {
            case 0:
                convertedX = -1;
                break;

            case 1:
                convertedX = 0;
                break;

            case 2:
                convertedX = 1;
                break;

            default:
                convertedX = int.MinValue;
                break;
        }

        switch (directionY)
        {
            case 0:
                convertedY = -1;
                break;

            case 1:
                convertedY = 0;
                break;

            case 2:
                convertedY = 1;
                break;

            default:
                convertedY = int.MinValue;
                break;
        }

        return new Vector2Int(convertedX, convertedY);
    }

    /// <summary>
    /// Process to reconvert the converted direction to a value that can be directly added to or subtracted from the index.
    /// For jump.
    /// </summary>
    private Vector2Int ConvertJumpDirection(int directionX, int directionY)
    {
        int convertedX, convertedY;

        switch (directionX)
        {
            case 0:
                convertedX = -2;
                break;

            case 1:
                convertedX = 0;
                break;

            case 2:
                convertedX = 2;
                break;

            default:
                convertedX = int.MinValue;
                break;
        }

        switch (directionY)
        {
            case 0:
                if(directionX == 0 || directionX == 2)
                {
                    convertedY = -1;
                }
                else
                {
                    convertedY = -2;
                }
                    break;

            case 1:
                convertedY = 0;
                break;

            case 2:
                if (directionX == 0 || directionX == 2)
                {
                    convertedY = 1;
                }
                else
                {
                    convertedY = 2;
                }
                break;

            default:
                convertedY = int.MinValue;
                break;
        }

        return new Vector2Int(convertedX, convertedY);
    }
}
