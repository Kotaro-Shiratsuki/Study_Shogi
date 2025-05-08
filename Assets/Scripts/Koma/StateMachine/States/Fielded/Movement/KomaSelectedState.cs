public class KomaSelectedState : KomaFieldedState
{
    public KomaSelectedState(KomaMovementStateMachine komaMovementStateMachine) : base(komaMovementStateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        StartAnimation(stateMachine.Koma.AnimationData.SelectedParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();

        StopAnimation(stateMachine.Koma.AnimationData.SelectedParameterHash);
    }
}
