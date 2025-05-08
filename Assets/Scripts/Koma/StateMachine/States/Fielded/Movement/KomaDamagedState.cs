public class KomaDamagedState : KomaFieldedState
{
    public KomaDamagedState(KomaMovementStateMachine komaMovementStateMachine) : base(komaMovementStateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        // Start aimation
        StartAnimation(stateMachine.Koma.AnimationData.DamagedParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();

        // Stop animation
        StopAnimation(stateMachine.Koma.AnimationData.DamagedParameterHash);
    }

    public override void OnAnimationExitEvent()
    {
        base.OnAnimationExitEvent();

        stateMachine.ChangeState(stateMachine.DefeatedState);
    }
}
