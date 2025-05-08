public class KomaWaitingState : KomaFieldedState
{
    public KomaWaitingState(KomaMovementStateMachine komaMovementStateMachine) : base(komaMovementStateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        StartAnimation(stateMachine.Koma.AnimationData.WaitingParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();
        StopAnimation(stateMachine.Koma.AnimationData.WaitingParameterHash);
        GameManager.Instance.CancelWaiting();
    }
}
