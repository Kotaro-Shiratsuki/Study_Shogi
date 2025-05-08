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
    public Vector3 CurrentWorldPosition { get; set; }
    public Vector2Int CurrentShogiPosition { get; set; }
    public Vector3 MoveTargetPosition { get; set; }

    /// <summary>
    /// 攻撃する際のダメージ倍率
    /// </summary>
    public float AttackDamageModifier { get; set; } = 1f;

    /// <summary>
    /// 被ダメージする際のダメージ倍率
    /// </summary>
    public float RecieveDamageModifier { get; set; } = 1f;

    /// <summary>
    /// ダメージ無効状態
    /// </summary>
    public bool SuperArmer { get; set; } = false;
}