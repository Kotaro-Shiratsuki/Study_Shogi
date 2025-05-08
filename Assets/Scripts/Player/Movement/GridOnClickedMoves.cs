using UnityEngine;

/// <summary>
/// Class of behaviour when a grid is clicked.
/// </summary>
public class GridOnClickedMoves
{
    private Player player;
    private GameManager manager;
    private PlayerOnClickMovements moves;
    private Vector2Int cachedGridPosition, clickedPosition;
    private string currentKey, cachedKey;

    public GridOnClickedMoves(Player player, PlayerOnClickMovements moves)
    {
        this.player = player;
        this.moves = moves;
        Init();
    }

    private void Init()
    {
        manager = player.Manager;
        cachedGridPosition = new Vector2Int(-1, -1);
        clickedPosition = new Vector2Int(-2, -2);
        currentKey = string.Empty;
        cachedKey = string.Empty;
    }

    internal void OnClicked(RaycastHit hit)
    {
        moves.currentGrid = hit.collider.GetComponent<ShogiGrid>();
        clickedPosition = moves.currentGrid.GetShogiPosition();

        // When you click on the same grid, it does nothing.
        if (clickedPosition == cachedGridPosition)
        {
            return;
        }

        MovementOnGridClicked();

        // Update cache.
        moves.cachedGrid = moves.currentGrid;
        cachedGridPosition = clickedPosition;

        Debug.Log("Grid clicked : " + cachedGridPosition.ToString());
    }

    internal void OnActionButtonClicked()
    {
        if (moves.cachedKoma != null)
        {
            moves.cachedKoma.UpdateTargetPosition(moves.currentGrid.GetGridPosition(), moves.currentGrid.GetShogiPosition());
            moves.cachedKoma.OnActionButtonClicked(moves.currentGrid.GetGridState(), player.IsEvolution());
        }
    }

    private void MovementOnGridClicked()
    {
        cachedKey = currentKey;
        currentKey = manager.ConvertIntPosToString(clickedPosition);
        GridState state = manager.GridDictionary.GetState(currentKey);

        switch(state)
        {
            case GridState.Movable:
            case GridState.Attackable:
            case GridState.MovableWithEv:
            case GridState.AttackableWithEv:
                WaitOnGrid();
                break;

            case GridState.Friend:
                MovementOnFriendSideGridClicked();
                break;

            case GridState.Enemy:
                MovementOnEnemySideGridClicked();
                break;

            default:
                MovementOnEmptyGridClicked();
                break;
        }
    }

    private void WaitOnGrid()
    {
        Debug.Log(cachedKey);
        if(manager.KomaDictionary.TryGetValue(cachedKey, out Koma clicked))
        {
            clicked.Wait();
        }
        else
        {
            player.WaitOnSideBoard();
        }

        player.ShowActionButtons();
    }

    private void MovementOnFriendSideGridClicked()
    {
        //if(player.CurrentSide != Player.Side.Friend)
        //{
        //    return;
        //}

        // Hide UI
        player.HideUiOnWaitCanceling();

        if (moves.cachedKoma != null)
        {
            // Rsest befor clicked.
            Owner owner = moves.cachedKoma.Owner;
            manager.ResetGridState(owner);
            
            ResetClickedKoma();
            player.ResetSelectedAnchor();
        }

        // Update grid state.
        manager.UpdateMovableGrid(clickedPosition);

        // Set current clicked koma's state.
        UpdateClickedKoma();
    }

    private void MovementOnEnemySideGridClicked()
    {
        //if (player.CurrentSide != Player.Side.Enemy)
        //{
        //    return;
        //}

        // Hide UI
        player.HideUiOnWaitCanceling();

        if (moves.cachedKoma != null)
        {
            // Rsest befor clicked.
            Owner owner = moves.cachedKoma.Owner;
            manager.ResetGridState(owner);

            ResetClickedKoma();
            player.ResetSelectedAnchor();
        }

        // Update grid state.
        manager.UpdateMovableGrid(clickedPosition);

        // Set current clicked koma's state.
        UpdateClickedKoma();
    }

    internal void MovementOnEmptyGridClicked()
    {
        // Hide UI
        player.HideUiOnWaitCanceling();

        if (moves.cachedKoma != null)
        {
            // Reset grid state.
            Owner owner = moves.cachedKoma.Owner;
            manager.ResetGridState(owner);
        }

        // Rsest befor clicked.
        ResetClickedKoma();
        player.ResetSelectedAnchor();
    }

    /// <summary>
    /// Update koma's clicked state.
    /// </summary>
    private void UpdateClickedKoma()
    {
        if(manager.KomaDictionary.TryGetValue(currentKey, out Koma clicked))
        {
            clicked.OnClicked();
            moves.cachedKoma = clicked;
        }
    }

    /// <summary>
    /// Remove koma's clicked state.
    /// </summary>
    internal void ResetClickedKoma()
    {
        if(manager.KomaDictionary.TryGetValue(cachedKey, out Koma befor))
        {
            befor.RemoveSelected();
            CacheClear();
        }
    }

    internal void ResetClickedPosition()
    {
        clickedPosition = new Vector2Int(int.MaxValue, int.MinValue);
    }

    internal void CacheClear()
    {
        cachedKey = string.Empty;
        cachedGridPosition = new Vector2Int(-1, -1);
    }

    internal void CachedKeyClear()
    {
        cachedKey = string.Empty;
        currentKey = string.Empty;
    }

    internal bool ShowActionButtons()
    {
        // Switching overlay effect.
        moves.cachedGrid.EffectSwitcher();
        moves.currentGrid.Selected();

        GridState state = moves.currentGrid.GetGridState();
        player.SwitchActionButtons(state);

        return state == GridState.MovableWithEv || state == GridState.AttackableWithEv;

    }
}