using UnityEngine;

public class KomaInitLayout
{
    /// <summary>
    /// 駒の初期配置
    /// </summary>
    public ID[,] InitLayout { get; private set; } = new ID[StaticMembers.BoardRow, StaticMembers.BoardColumn]
    {
        // 敵陣1段目
        { ID.Kyosya, ID.Keima, ID.Ginsyo, ID.Kinsyo, ID.Gyoku, ID.Kinsyo, ID.Ginsyo, ID.Keima, ID.Kyosya },

        // 敵陣2段目
        { ID.None, ID.Hisya, ID.None, ID.None, ID.None, ID.None, ID.None, ID.Kakugyo, ID.None },

        // 敵陣3段目
        { ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo },

        // 中立陣
        { ID.None, ID.None, ID.None, ID.None, ID.None, ID.None, ID.None, ID.None, ID.None },
        { ID.None, ID.None, ID.None, ID.None, ID.None, ID.None, ID.None, ID.None, ID.None },
        { ID.None, ID.None, ID.None, ID.None, ID.None, ID.None, ID.None, ID.None, ID.None },

        // 自陣7段目
        { ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo },

        // 自陣8段目
        { ID.None, ID.Kakugyo, ID.None, ID.None, ID.None, ID.None, ID.None, ID.Hisya, ID.None },

        // 自陣9段目
        { ID.Kyosya, ID.Keima, ID.Ginsyo, ID.Kinsyo, ID.Gyoku, ID.Kinsyo, ID.Ginsyo, ID.Keima, ID.Kyosya }
    };

    public ID GetID(Vector2Int index)
    {
        if (GameManager.Instance.CheckArrayRange(index.x, index.y, InitLayout.GetLength(0), InitLayout.GetLength(1)))
        {
            return InitLayout[index.x, index.y];
        }

        return ID.None;
    }
}
