public class KomaIdleOnSideState : KomaCupturedState
{
    public KomaIdleOnSideState(KomaMovementStateMachine komaMovementStateMachine) : base(komaMovementStateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        StartAnimation(stateMachine.Koma.AnimationData.IdleOnSideParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();

        StopAnimation(stateMachine.Koma.AnimationData.IdleOnSideParameterHash);
    }
}
