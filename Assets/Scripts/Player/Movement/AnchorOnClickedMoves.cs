using UnityEngine;

public class AnchorOnClickedMoves
{
    private Player player;
    private GameManager manager;
    private PlayerOnClickMovements moves;
    private int currentIndex, cachedIndex;
    private SideBoard friendSide, enemySide;

    public AnchorOnClickedMoves(Player player, PlayerOnClickMovements moves)
    {
        this.player = player;
        this.moves = moves;
        Init();
    }

    private void Init()
    {
        manager = player.Manager;
        currentIndex = int.MinValue;
        cachedIndex = int.MaxValue;
        friendSide = manager.FriendSideBoard;
        enemySide = manager.EnemySideBoard;
    }

    internal void OnClicked(RaycastHit hit)
    {
        moves.currentAnchor = hit.collider.GetComponent<Anchor>();
        currentIndex = moves.currentAnchor.Index;

        // When you click on the same anchor, it does nothing.
        if (currentIndex == cachedIndex)
        {
            return;
        }

        MovementOnAnchorClicked();

        // Update cache.
        moves.cachedAnchor = moves.currentAnchor;
        cachedIndex = currentIndex;

        Debug.Log("Anchor clicked : " + cachedIndex.ToString());
    }

    internal void OnActionButtonClicked()
    {
        Koma ejected;
        Debug.Log(cachedIndex.ToString());
        if(moves.cachedAnchor.IsFriend)
        {
            ejected = friendSide.EjectSelectedReserver(cachedIndex);
        }
        else
        {
            ejected = enemySide.EjectSelectedReserver(cachedIndex);
        }

        ejected.UpdateTargetPosition(moves.currentGrid.GetGridPosition(), moves.currentGrid.GetShogiPosition());
        ejected.MoveOnSide();
    }

    private void MovementOnAnchorClicked()
    {
        if(moves.currentAnchor.State != AnchorState.Idle)
        {
            return;
        }

        MovementOnIdleAnchor();
    }

    internal void WaitOnSideBoard()
    {
        if(moves.cachedAnchor == null)
        {
            return;
        }

        Koma reserved;

        if (moves.cachedAnchor.IsFriend)
        {
            reserved = friendSide.GetKomaOnAnchor(cachedIndex);
        }
        else
        {
            reserved = enemySide.GetKomaOnAnchor(cachedIndex);
        }

        reserved.WaitOnSide();
    }

    private void MovementOnIdleAnchor()
    {
        player.ResetClickedKoma();

        if(moves.currentAnchor.IsFriend)
        {
            friendSide.OnClickedIdleAnchor(currentIndex);
        }
        else
        {
            enemySide.OnClickedIdleAnchor(currentIndex);
        }

        manager.UpdateMovableGridOnSide(moves.currentAnchor.ReserversID, moves.currentAnchor.IsFriend);
    }

    internal void MoveOnSideBoard()
    {
        if (moves.cachedAnchor == null)
        {
            return;
        }

        Koma reserved;

        if(moves.cachedAnchor.IsFriend)
        {
            reserved = friendSide.GetKomaOnAnchor(cachedIndex);
        }
        else
        {
            reserved = enemySide.GetKomaOnAnchor(cachedIndex);
        }


    }

    internal void ResetSelectedAnchor()
    {
        if (moves.cachedAnchor == null)
        {
            return;
        }

        if (moves.cachedAnchor.IsFriend)
        {
            friendSide.ResetSelectedAnchor();
        }
        else
        {
            enemySide.ResetSelectedAnchor();
        }

        CacheClear();
    }

    internal void CacheClear()
    {
        cachedIndex = int.MinValue;
        moves.cachedAnchor = null;
    }
}