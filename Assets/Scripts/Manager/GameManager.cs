using UnityEngine;

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

    [field: Header("Dictionaries")]
    public GridDictionaryController GridDictionary { get; private set; }
    public KomaDictionaryController KomaDictionary { get; private set; }
    public SideBoard FriendSideBoard { get; private set; }
    public SideBoard EnemySideBoard { get; private set; }

    [field: Header("UI Canvas")]
    [field: SerializeField]
    public CanvasController Canvas { get; private set; }

    internal MovementDataParser movementDataParser;
    internal Index indexController;
    internal GridStatusController gridStatusController;

    protected override void Awake()
    {
        base.Awake();

        indexController = new Index();
        GridDictionary = new GridDictionaryController(indexController);
        KomaDictionary = new KomaDictionaryController(indexController);
        movementDataParser = new MovementDataParser();
        gridStatusController = new GridStatusController(this);
    }

    public void SetSideBoard(SideBoard friend, SideBoard enemy)
    {
        FriendSideBoard = friend;
        EnemySideBoard = enemy;
    }

    public string ConvertIntPosToString(Vector2Int shogiPos)
    {
        return indexController.ParseShogiPosition(shogiPos);
    }

    public string ParseIndexToPos(Vector2Int index)
    {
        return indexController.ParseIndexToShogiPosition(index);
    }

    public Vector2Int ParsePosToIndex(string pos)
    {
        return indexController.ParseShogiPositionToIndex(pos);
    }

    /// <summary>
    /// Parse two dimentional array Index to shogi grid number.
    /// </summary>
    public Vector2Int GetPosAsInt(Vector2Int index)
    {
        return indexController.ParseIndexToShogiPosNum(index);
    }

    public void UpdateOnMoved(Vector2Int prePosition, Vector2Int postPosition)
    {
        UpdateKomaDictionary(prePosition, postPosition);
        UpdateGridDictionaryOnMoved(prePosition, postPosition);
    }

    public void UpdateOnCuptured(Vector2Int prePosition)
    {
        Koma koma;
        string pre = indexController.ParseShogiPosition(prePosition);

        if (KomaDictionary.TryGetValue(pre, out koma))
        {
            KomaDictionary.Remove(pre);

            if (koma.Owner == Owner.Player)
            {
                FriendSideBoard.UpdateReserves(koma);
            }
            else
            {
                EnemySideBoard.UpdateReserves(koma);
            }
        }

        KomaRemovedOnGrid(pre);
    }

    /// <summary>
    /// Update koma dictionary afetr move.
    /// </summary>
    public void UpdateKomaDictionary(Vector2Int prePosition, Vector2Int postPosition)
    {
        string pre = indexController.ParseShogiPosition(prePosition);
        string post = indexController.ParseShogiPosition(postPosition);
        Vector2Int index = indexController.ParseShogiPositionToIndex(post);

        KomaDictionary.UpdateKomaDictionary(pre, post, index);
    }

    public void AddKomaDictionaryBySide(Koma koma, Vector2Int shogiPosition)
    {
        string pos = indexController.ParseShogiPosition(shogiPosition);
        if (!KomaDictionary.ContainsKey(pos))
        {
            KomaDictionary.Add(pos, indexController.ParseShogiPositionToIndex(pos), koma);
            GridDictionary.GetValue(pos).ChangeState(
            KomaDictionary.GetValue(pos).Owner == Owner.Player ? GridState.Friend : GridState.Enemy);
        }
    }

    public void UpdateGridDictionaryOnMoved(Vector2Int prePosition, Vector2Int postPosition)
    {
        string pre = indexController.ParseShogiPosition(prePosition);
        string post = indexController.ParseShogiPosition(postPosition);

        GridDictionary.GetValue(pre).ChangeState(GridState.Empty);
        GridDictionary.GetValue(post).ChangeState(
            KomaDictionary.GetValue(post).Owner == Owner.Player? GridState.Friend : GridState.Enemy);
    }

    /// <summary>
    /// Get koma's movement area in clicked position.
    /// And update empty grid to movable.
    /// </summary>
    public void UpdateMovableGrid(Vector2Int clicked)
    {
        gridStatusController.UpdateMovableGrid(clicked);
    }

    public void UpdateMovableGridOnSide(ID id, bool isFriend)
    {
        gridStatusController.UpdateMovableGridOnSide(id, isFriend);
    }

    /// <summary>
    /// Remove 'Movable' state from movablegrid.
    /// </summary>
    public void ResetGridState(Owner owner)
    {
        GridDictionary.ResetGridState(owner);
    }

    /// <summary>
    /// Check is position out of range.
    /// </summary>
    public bool CheckArrayRange(int x, int y, int rows, int cols)
    {
        return x >= 0 && x < rows && y >= 0 && y < cols;
    }

    public void CancelWaiting()
    {
        Canvas.ActionButtonsPanel.RemoveAllListeners();
        Canvas.HideButtons();
    }

    private void KomaRemovedOnGrid(string key)
    {
        if(GridDictionary.TryGetValue(key, out var grid))
        {
            grid.KomaRemovedOnGrid();
        }
    }
}
