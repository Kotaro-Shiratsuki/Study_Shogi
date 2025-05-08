public class KomaDefeatedState : KomaFieldedState
{
    public KomaDefeatedState(KomaMovementStateMachine komaMovementStateMachine) : base(komaMovementStateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        // Start animation
        StartAnimation(stateMachine.Koma.AnimationData.DefeatedParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();

        // StopAnimation
        StopAnimation(stateMachine.Koma.AnimationData.DefeatedParameterHash);
    }


    public override void OnAnimationExitEvent()
    {
        base.OnAnimationExitEvent();
        Defeated();
    }

    private void Defeated()
    {
        stateMachine.Koma.OnDefeated();
    }
}
