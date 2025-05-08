public class KomaStoppingState : KomaFieldedState
{
    public KomaStoppingState(KomaMovementStateMachine komaMovementStateMachine) : base(komaMovementStateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        // Start animation
        StartAnimation(stateMachine.Koma.AnimationData.StoppingParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();

        // Stop animation
        StopAnimation(stateMachine.Koma.AnimationData.StoppingParameterHash);
    }

    public override void OnAnimationExitEvent()
    {
        base.OnAnimationExitEvent();

        stateMachine.ChangeState(stateMachine.IdlingState);
    }
}
