using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KomaSkillsData
{
    [field: SerializeField]
    public ActiveSkill ActiveSkillData { get; private set; }

    [field: SerializeField]
    public PassiveSkill PassiveSkillData { get; private set; }
}