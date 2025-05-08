public class KomaCupturedState : KomaBasementState
{
    public KomaCupturedState(KomaMovementStateMachine komaMovementStateMachine) : base(komaMovementStateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        StartAnimation(stateMachine.Koma.AnimationData.CupturedParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();

        StopAnimation(stateMachine.Koma.AnimationData.CupturedParameterHash);
    }
}
