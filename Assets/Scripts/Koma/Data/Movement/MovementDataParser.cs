using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// komaMovementDataを盤面上の移動に置き換えるためのパーサークラス
/// 移動範囲を 1 とした 9*9 の配列を返す
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
    /// 移動先候補マスを9*9配列として返す処理
    /// </summary>
    public int[,] GetMovementArea(List<MovesListRow> moves, Vector2Int pos, bool isReverse = false)
    {
        // 毎回0クリア
        System.Array.Clear(movementAreas, 0, movementAreas.Length);
        int[,] moveDirection = new int[directionColumn, directionRow];

        // MovementDataで設定したMoveTypeのリストをintに変換
        moveDirection = ParseMovetypeToDirection(moves);

        // 変換されたMoveType毎に、自身を中心として移動範囲配列を作成
        ParseMoveDirectionToMovableAreas(pos, moveDirection, isReverse);

        return movementAreas;
    }

    /// <summary>
    /// MoveTypeをint配列として変換する処理
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
    /// 変換されたMoveDirectionをもとに、自身を中心として移動範囲を代入する処理
    /// </summary>
    private void ParseMoveDirectionToMovableAreas(Vector2Int pos, int[,] direction, bool isReverse)
    {
        Vector2Int center = GameManager.Instance.ParseShogiPosToTwoDimentionalIndex(pos);

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
    /// MoveDirection == walk の時の代入処理
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
    /// MoveDirection == fly の時の代入処理
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
    /// MoveDirection == jump の時の代入処理
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
    /// 変換された方向を、直接要素インデックスと足し引きできる値に再変換する処理
    /// WalkとFly用。
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
    /// 変換された方向を、直接要素インデックスと足し引きできる値に再変換する処理
    /// Jump用特殊処理。Jumpの挙動を変えたいときは、この処理に変更を加える。
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
