public class KomaIdlingState : KomaFieldedState
{
    public KomaIdlingState(KomaMovementStateMachine komaMovementStateMachine) : base(komaMovementStateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        StartAnimation(stateMachine.Koma.AnimationData.IdlingParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();

        StopAnimation(stateMachine.Koma.AnimationData.IdlingParameterHash);
    }
}