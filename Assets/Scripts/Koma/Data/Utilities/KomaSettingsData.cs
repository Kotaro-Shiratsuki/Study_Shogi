using UnityEngine;
[System.Serializable]
public class KomaSettingsData
{
    [field: Header("Settings")]
    [field: SerializeField]
    public MovemSetting MoveData { get; private set; }

    [field: SerializeField]
    public AttackSetting AttackData { get; private set; }

    [field: SerializeField]
    public CupturedSetting CupturedData { get; private set; }
}
