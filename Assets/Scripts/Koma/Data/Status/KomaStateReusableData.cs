using UnityEngine;

public class KomaStateReusableData
{
    // Status
    [field: Range(0, 6)]
    public int CurrentLevel { get; set; } = 1;
    public int CurrentHitPoint {  get; set; }
    public int CurrentAttackPoint { get; set; }
    public int CurrentExp { get; set; }
    public int AmountOfExp { get; set; } = 0;
    public int ExpToNextLevel { get; set; } = int.MaxValue;
    public bool IsEvolved { get; set; } = false;

    // Movement
    public float MoveSpeedModifier { get; set; } = 1.0f;
    public float JumpHeightModifier { get; set; } = 1.0f;
    public Vector3 CurrentWorldPosition { get; set; }
    public Vector2Int CurrentShogiPosition { get; set; }
    public Vector3 MoveTargetPosition { get; set; }

    
    public float AttackDamageModifier { get; set; } = 1f;
    public float RecieveDamageModifier { get; set; } = 1f;
    public bool SuperArmer { get; set; } = false;
}