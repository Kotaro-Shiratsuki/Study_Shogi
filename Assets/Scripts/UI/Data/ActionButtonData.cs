using UnityEngine;

[System.Serializable]
public class ActionButtonData
{
    [field: SerializeField]
    public Color MoveButtonColor { get; private set; } = Color.cyan;

    [field: SerializeField]
    public string MoveButtonText { get; private set; } = "進軍";

    [field: SerializeField]
    public Color AttackButtonColor { get; private set; } = Color.magenta;

    [field: SerializeField]
    public string AttackButtonText { get; private set; } = "合戦";

    [field: SerializeField]
    public Color CancelButtonColor { get; private set; } = Color.gray;

    [field: SerializeField]
    public string CancelButtonText { get; private set; } = "中止";

    [field: SerializeField]
    public string EvolutionToggleText { get; private set; } = "成駒";

    [field: SerializeField]
    public Color ActiveColor { get; private set; } = Color.white;

    [field: SerializeField]
    public Color DeactiveColor { get; private set; } = Color.gray;
}
