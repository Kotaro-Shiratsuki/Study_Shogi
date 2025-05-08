using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KomaStatusData
{
    [field: SerializeField]
    public ID KomaID { get; private set; }

    [field: SerializeField]
    public int MaxLevel { get; private set; } = 5;

    [field: SerializeField]
    public List<StatusParam> StatusTable { get; private set; } = new List<StatusParam>();


    [field: SerializeField]
    public bool CanEvolution { get; private set; } = true;

    [field: SerializeField][Tooltip("The status when this 'Koma' be evolved and reach to max levels.")]
    public StatusParam MaxStatus { get; private set; }

    [field: SerializeField]
    public int ExHitPoint { get; private set; }

    [field: SerializeField]
    public int ExAttackPoint { get; private set; }
}

[System.Serializable]
public class StatusParam
{
    [field: SerializeField]
    public int ExpToNextLevel { get; private set; }

    [field: SerializeField]
    public int HitPoints { get; private set; }

    [field: SerializeField]
    public int AttackPoints { get; private set; }

    [field: SerializeField]
    public int Exp { get; private set; }
}

[System.Serializable]
public enum ID
{
    None    = -1,
    Huhyo   = 0,
    Kyosya  = 1,
    Keima   = 2,
    Ginsyo  = 3,
    Kinsyo  = 4,
    Kakugyo = 5,
    Hisya   = 6,
    Gyoku   = 7,
    Size,
}