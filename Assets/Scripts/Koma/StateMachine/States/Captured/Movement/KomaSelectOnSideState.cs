public class KomaSelectOnSideState : KomaCupturedState
{
    public KomaSelectOnSideState(KomaMovementStateMachine komaMovementStateMachine) : base(komaMovementStateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        StartAnimation(stateMachine.Koma.AnimationData.SelectOnSideParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();

        StopAnimation(stateMachine.Koma.AnimationData.SelectOnSideParameterHash);
    }
}
