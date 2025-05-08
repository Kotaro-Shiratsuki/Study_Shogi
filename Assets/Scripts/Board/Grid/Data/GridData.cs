using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridData
{
    /// <summary>
    /// マス目のワールド座標
    /// </summary>
    [field: SerializeField]
    public Vector3 WorldPosition { get; private set; }

    /// <summary>
    /// マス目の将棋盤的な数え方による位置
    /// </summary>
    [field: SerializeField]
    public Vector2Int ShogiPosition { get; private set; }

    /// <summary>
    /// 配列の要素としての番号
    /// </summary>
    [field: SerializeField]
    public Vector2Int IndexNumber {  get; private set; }

    /// <summary>
    /// マスの状態。空き、駒あり、合戦中のどれか
    /// </summary>
    [field: SerializeField]
    public GridState State { get; private set; }

    /// <summary>
    /// マスの所属している陣地
    /// </summary>
    [field: SerializeField]
    public GridRegion Region { get; private set; }

    [field: SerializeField]
    public List<GameObject> Rulers { get; private set; } = new List<GameObject>();

    #region Setter Methods
    public void SetWorldPosition(Vector3 worldPosition)
    {
        this.WorldPosition = worldPosition;
    }

    public void SetShogiPosition(Vector2Int shogiPosition)
    {
        this.ShogiPosition = shogiPosition;
    }

    public void SetElementNumber(Vector2Int element)
    {
        this.IndexNumber = element;
    }

    public void SetState(GridState state)
    {
        this.State = state;
    }

    public void SetRegion(GridRegion region)
    {
        this.Region = region;
    }

    public void SetRulers(GameObject ruler)
    {
        Rulers.Clear();
        Rulers.Add(ruler);
    }
    #endregion
}

[System.Serializable]
public enum GridState
{
    None,
    Empty,
    Ruled,
    War,
    Movable,
    Size,
}

[System.Serializable]
public enum GridRegion
{
    None,
    Friend,
    Neutral,
    Enemy,
    Size,
}