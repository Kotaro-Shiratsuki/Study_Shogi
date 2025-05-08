public class KomaMoveToBoardState : KomaCupturedState
{
    private MovemSetting moveData;

    public KomaMoveToBoardState(KomaMovementStateMachine komaMovementStateMachine) : base(komaMovementStateMachine)
    {
        moveData = settingsData.MoveData;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        // Initialize reusable data
        stateMachine.ReusableData.MoveSpeedModifier = moveData.MoveSpeed;
        stateMachine.ReusableData.JumpHeightModifier = moveData.JumpHeight;
        stateMachine.ReusableData.MoveTargetPosition = stateMachine.Koma.TargetWorldPosition;

        InitMovementParameter();

        StartAnimation(stateMachine.Koma.AnimationData.MoveToBoardParameterHash);
    }

    public override void NormalUpdate()
    {
        base.NormalUpdate();

        MoveToBoard();
    }

    public override void OnExit()
    {
        base.OnExit();

        StopAnimation(stateMachine.Koma.AnimationData.MoveToBoardParameterHash);
    }
}
