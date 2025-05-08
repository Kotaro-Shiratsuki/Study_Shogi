using UnityEngine;

[System.Serializable]
public class Anchor : MonoBehaviour
{
    [field: SerializeField]
    public int Index { get; private set; }

    [field: SerializeField]
    public Vector3 AnchorPoint { get; private set; }

    [field: SerializeField]
    public ID ReserversID { get; private set; } = ID.None;

    [field: SerializeField]
    public AnchorState State { get; private set; } = AnchorState.None;

    [field: SerializeField]
    public bool IsFriend { get; private set; }

    private void Awake()
    {
        InitAnchor();
    }

    public void InitAnchor()
    {
        AnchorPoint = transform.position;
    }

    public void SetIndex(int index)
    {
        Index = index;
    }

    public void SetState(AnchorState state)
    {
        State = state;
    }

    public void UpdateID(ID id)
    {
        ReserversID = id;
    }

    public void BeFriend(bool isFriend)
    {
        IsFriend = isFriend;
    }
}

public enum AnchorState
{
    None,
    Empty,
    Idle,
    Selected,
    Size
}