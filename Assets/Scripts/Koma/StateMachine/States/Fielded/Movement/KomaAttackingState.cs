using UnityEngine;

public class KomaAttackingState : KomaFieldedState
{
    private Vector2Int prePosition;
    private ShogiGrid grid;
    private AttackSetting attackData;


    public KomaAttackingState(KomaMovementStateMachine komaMovementStateMachine) : base(komaMovementStateMachine)
    {
        attackData = settingsData.AttackData;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        // Initialize reusable data
        stateMachine.ReusableData.MoveSpeedModifier = attackData.MoveSpeed;
        stateMachine.ReusableData.JumpHeightModifier = attackData.JumpHeight;
        stateMachine.ReusableData.MoveTargetPosition = stateMachine.Koma.TargetWorldPosition;

        // Initialize position data
        prePosition = stateMachine.ReusableData.CurrentShogiPosition;

        StartAnimation(stateMachine.Koma.AnimationData.AttackingParameterHash);

        InitMovementParameter();

        string key = GameManager.Instance.ConvertIntPosToString(stateMachine.Koma.TargetShogiPosition);
        if (GameManager.Instance.KomaDictionary.TryGetValue(key, out var target))
        {
            target.OnAttacked();
        }

        grid = GameManager.Instance.GridDictionary.GetValue(key);
    }

    public override void NormalUpdate()
    {
        base.NormalUpdate();

        if(grid.GridData.State == GridState.Empty)
        {
            Move();
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        UpdateDictionary();

        StopAnimation(stateMachine.Koma.AnimationData.AttackingParameterHash);
    }

    private void UpdateDictionary()
    {
        GameManager.Instance.UpdateOnMoved(prePosition, stateMachine.ReusableData.CurrentShogiPosition);
    }
}
