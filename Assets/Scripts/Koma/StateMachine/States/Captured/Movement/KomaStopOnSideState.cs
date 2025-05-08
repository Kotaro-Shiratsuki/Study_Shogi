public class KomaStopOnSideState : KomaCupturedState
{
    public KomaStopOnSideState(KomaMovementStateMachine komaMovementStateMachine) : base(komaMovementStateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        StartAnimation(stateMachine.Koma.AnimationData.StopOnSideParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();

        StopAnimation(stateMachine.Koma.AnimationData.StopOnSideParameterHash);
    }

    public override void OnAnimationExitEvent()
    {
        base.OnAnimationExitEvent();

        stateMachine.ChangeState(stateMachine.IdleOnSideState);
    }
}
