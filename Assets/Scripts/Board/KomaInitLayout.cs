using UnityEngine;

public class KomaInitLayout
{
    /// <summary>
    /// Initial layout of Koma.
    /// </summary>
    public ID[,] InitLayout { get; private set; } = new ID[StaticMembers.BoardRow, StaticMembers.BoardColumn]
    {
        // The 0 row.
        { ID.Kyosya, ID.Keima, ID.Ginsyo, ID.Kinsyo, ID.Gyoku, ID.Kinsyo, ID.Ginsyo, ID.Keima, ID.Kyosya },

        // The 1 row.
        { ID.None, ID.Hisya, ID.None, ID.None, ID.None, ID.None, ID.None, ID.Kakugyo, ID.None },

        // The 2 row.
        { ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo },

        // middle
        { ID.None, ID.None, ID.None, ID.None, ID.None, ID.None, ID.None, ID.None, ID.None },
        { ID.None, ID.None, ID.None, ID.None, ID.None, ID.None, ID.None, ID.None, ID.None },
        { ID.None, ID.None, ID.None, ID.None, ID.None, ID.None, ID.None, ID.None, ID.None },

        // The 6 row.
        { ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo, ID.Huhyo },

        // The 7 row.
        { ID.None, ID.Kakugyo, ID.None, ID.None, ID.None, ID.None, ID.None, ID.Hisya, ID.None },

        // The 8 row.
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
