public class KomaStopOnBoardState : KomaCupturedState
{
    public KomaStopOnBoardState(KomaMovementStateMachine komaMovementStateMachine) : base(komaMovementStateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        StartAnimation(stateMachine.Koma.AnimationData.StopOnBoardParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();

        GameManager.Instance.AddKomaDictionaryBySide(stateMachine.Koma, stateMachine.Koma.TargetShogiPosition);
        StopAnimation(stateMachine.Koma.AnimationData.StopOnBoardParameterHash);
    }

    public override void OnAnimationExitEvent()
    {
        base.OnAnimationExitEvent();

        stateMachine.ChangeState(stateMachine.IdlingState);
    }
}
