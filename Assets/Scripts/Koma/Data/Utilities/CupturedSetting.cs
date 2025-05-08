using UnityEngine;

[System.Serializable]
public class CupturedSetting
{
    [field: SerializeField]
    public float MoveSpeed { get; private set; } = 1.0f;

    [field: SerializeField]
    public float JumpHeight { get; private set; } = 1.0f;
}
