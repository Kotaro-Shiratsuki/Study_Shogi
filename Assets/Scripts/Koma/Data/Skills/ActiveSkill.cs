using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActiveSkill
{
    [field: SerializeField]
    public int ActiveSkillWaitTurn { get; private set; }

    [field: SerializeField]
    public int ActiveSkillRecastTurn { get; private set; }

    [field: SerializeField]
    public bool IsUseInUsual { get; private set; }

    [field: SerializeField]
    public bool IsUseInCombat { get; private set; }
}