using UnityEngine;

[System.Serializable]
public class GridData
{
    /// <summary>
    /// World position.
    /// </summary>
    [field: SerializeField]
    public Vector3 WorldPosition { get; private set; }

    /// <summary>
    /// Position of counting on the Shogi Board.
    /// </summary>
    [field: SerializeField]
    public Vector2Int ShogiPosition { get; private set; }

    /// <summary>
    /// Array index.
    /// </summary>
    [field: SerializeField]
    public Vector2Int IndexNumber {  get; private set; }

    /// <summary>
    /// Grid state.
    /// </summary>
    [field: SerializeField]
    public GridState State { get; private set; }

    /// <summary>
    /// The region to witch the grid belong.
    /// </summary>
    [field: SerializeField]
    public GridRegion Region { get; private set; }

    #region Setter Methods
    public void SetWorldPosition(Vector3 worldPosition)
    {
        this.WorldPosition = worldPosition;
    }

    public void SetShogiPosition(Vector2Int shogiPosition)
    {
        this.ShogiPosition = shogiPosition;
    }

    public void SetIndexNumber(Vector2Int element)
    {
        this.IndexNumber = element;
    }

    public void UpdateState(GridState state)
    {
        this.State = state;
    }

    public void SetRegion(GridRegion region)
    {
        this.Region = region;
    }
    #endregion
}

[System.Serializable]
public enum GridState
{
    None,
    Empty,
    Friend,
    Enemy,
    War,
    Movable,
    MovableWithEv,
    Attackable,
    AttackableWithEv,
    Blocked,
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