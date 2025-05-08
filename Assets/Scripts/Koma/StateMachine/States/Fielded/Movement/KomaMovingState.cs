using UnityEngine;

public class KomaMovingState : KomaFieldedState
{
    private Vector2Int prePosition;
    private MovemSetting moveData;

    public KomaMovingState(KomaMovementStateMachine komaMovementStateMachine) : base(komaMovementStateMachine)
    {
        moveData = settingsData.MoveData;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        prePosition = stateMachine.ReusableData.CurrentShogiPosition;

        // Initialize reusable data
        stateMachine.ReusableData.MoveSpeedModifier = moveData.MoveSpeed;
        stateMachine.ReusableData.JumpHeightModifier = moveData.JumpHeight;
        stateMachine.ReusableData.MoveTargetPosition = stateMachine.Koma.TargetWorldPosition;

        InitMovementParameter();

        // Start animtion
        StartAnimation(stateMachine.Koma.AnimationData.MovingParameterHash);
    }

    public override void NormalUpdate()
    {
        base.NormalUpdate();
        Move();
    }

    public override void OnExit()
    {
        base.OnExit();

        // Update koma dictionary
        UpdateDictionary();

        // Stop animation
        StopAnimation(stateMachine.Koma.AnimationData.MovingParameterHash);
    }

    private void UpdateDictionary()
    {
        GameManager.Instance.UpdateOnMoved(prePosition, stateMachine.ReusableData.CurrentShogiPosition);
    }
}
