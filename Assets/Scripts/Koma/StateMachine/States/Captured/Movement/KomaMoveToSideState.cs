using UnityEngine;

public class KomaMoveToSideState : KomaCupturedState
{
    private Vector2Int prePosition;
    private CupturedSetting cupturedData;

    public KomaMoveToSideState(KomaMovementStateMachine komaMovementStateMachine) : base(komaMovementStateMachine)
    {
        cupturedData = settingsData.CupturedData;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        prePosition = stateMachine.ReusableData.CurrentShogiPosition;
        stateMachine.ReusableData.MoveTargetPosition = stateMachine.Koma.TargetWorldPosition;
        stateMachine.ReusableData.MoveSpeedModifier = cupturedData.MoveSpeed;
        stateMachine.ReusableData.JumpHeightModifier = cupturedData.JumpHeight;

        InitMovementParameter();

        StartAnimation(stateMachine.Koma.AnimationData.MoveToSideParameterHash);
    }

    public override void NormalUpdate()
    {
        base.NormalUpdate();

        MoveToSide();
    }

    public override void OnExit()
    {
        base.OnExit();

        SetBaseStatusData();
        UpdateDictionary();

        StopAnimation(stateMachine.Koma.AnimationData.MoveToSideParameterHash);
    }

    private void UpdateDictionary()
    {
        GameManager.Instance.UpdateOnCuptured(prePosition);
    }
}
