using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Movingtype
{
    None,
    Walk,
    Fly,
    Jump
}


[System.Serializable]
public class KomaMovementData
{
    [field:SerializeField]
    public List<MovesListRow> DefaultMoving { get; private set; } = new List<MovesListRow>() { new MovesListRow(),  new MovesListRow(), new MovesListRow() };

    [field: SerializeField]
    public List<MovesListRow> ExtraMoving { get; private set; } = new List<MovesListRow>() { new MovesListRow(), new MovesListRow(), new MovesListRow() };
}

[System.Serializable]
public class MovesListRow
{
    [field: SerializeField]
    public List<Movingtype> MovingListRow { get; private set; } = new List<Movingtype>() { Movingtype.None, Movingtype.None, Movingtype.None };
}
