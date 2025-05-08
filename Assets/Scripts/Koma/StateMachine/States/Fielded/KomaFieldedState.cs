/// <summary>
/// 盤面上にある状態
/// 盤面上でのほかの全ての状態は、このクラスを継承される
/// </summary>
public class KomaFieldedState : KomaBasementState
{
    public KomaFieldedState(KomaMovementStateMachine komaMovementStateMachine) : base(komaMovementStateMachine)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();

        StartAnimation(stateMachine.Koma.AnimationData.FieldedParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();

        StopAnimation(stateMachine.Koma.AnimationData.FieldedParameterHash);
    }
}