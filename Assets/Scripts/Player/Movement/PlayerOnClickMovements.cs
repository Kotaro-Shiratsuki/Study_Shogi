using UnityEngine;

/// <summary>
/// Class of action for player's click action.
/// </summary>
public class PlayerOnClickMovements
{
    private float distanceOfRay;
    private LayerMask gridLayer;
    private LayerMask anchorLayer;
    private GridOnClickedMoves gridMovement;
    private AnchorOnClickedMoves anchorMovement;

    #region Caches
    internal Koma cachedKoma;
    internal ShogiGrid currentGrid, cachedGrid;
    internal Anchor currentAnchor, cachedAnchor;
    #endregion

    public PlayerOnClickMovements(Player player)
    {
        gridMovement = new GridOnClickedMoves(player, this);
        anchorMovement = new AnchorOnClickedMoves(player, this);

        gridLayer = player.GridLayer;
        anchorLayer = player.AnchorLayer;
        distanceOfRay = player.DistanceOfRay;
    }

    /// <summary>
    /// Cast ray to clicked position.
    /// </summary>
    internal void CastRayOnClicked(Vector2 mousePosition, Ray ray)
    {
        RaycastHit hit = new RaycastHit();

        if(Physics.Raycast(ray, out hit, distanceOfRay, gridLayer))
        {
            gridMovement.OnClicked(hit);
        }
        else if(Physics.Raycast(ray, out hit, distanceOfRay, anchorLayer))
        {
            anchorMovement.OnClicked(hit);
            gridMovement.ResetClickedPosition();
            gridMovement.CachedKeyClear();
        }
    }

    internal void OnActionButtonClicked()
    {
        if(cachedAnchor != null)
        {
            anchorMovement.OnActionButtonClicked();
            return;
        }
        else
        {
            gridMovement.OnActionButtonClicked();
            return;
        }
    }

    internal void OnCancelButtonClicked()
    {
        gridMovement.MovementOnEmptyGridClicked();
    }

    internal void WaitOnSideBoard()
    {
        anchorMovement.WaitOnSideBoard();
    }

    internal bool ShowActionButtons()
    {
        return gridMovement.ShowActionButtons();
    }

    internal void ResetClickedKoma()
    {
        gridMovement.ResetClickedKoma();
    }

    internal void ResetSelectedAnchor()
    {
        anchorMovement.ResetSelectedAnchor();
    }
}
