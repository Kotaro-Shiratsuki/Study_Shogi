using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KomaMovingState : KomaFieldedState
{
    private Vector3 currentPosition;
    private Vector2Int prePosition;

    private float duration = 0.75f;
    private float elapsedTime = 0.0f;
    public KomaMovingState(KomaMovementStateMachine komaMovementStateMachine) : base(komaMovementStateMachine)
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();

        elapsedTime = 0.0f;
        currentPosition = stateMachine.ReusableData.CurrentWorldPosition;
        prePosition = stateMachine.ReusableData.CurrentShogiPosition;

        UpdateTargetPosition();
    }

    public override void NormalUpdate()
    {
        base.NormalUpdate();

        Move();
    }

    public override void OnExit()
    {
        base.OnExit();

        elapsedTime = 0.0f;
        UpdateDictionary();
    }

    private void UpdateTargetPosition()
    {
        stateMachine.ReusableData.MoveTargetPosition = stateMachine.Koma.TargetWorldPosition;
    }

    private void UpdateDictionary()
    {
        GameManager.Instance.UpdateOnMoved(prePosition, stateMachine.ReusableData.CurrentShogiPosition);
    }

    private void Move()
    {
        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            stateMachine.Koma.transform.position = Vector3.Lerp(currentPosition, stateMachine.ReusableData.MoveTargetPosition, t);
        }
        else
        {
            stateMachine.Koma.transform.position = stateMachine.ReusableData.MoveTargetPosition;
            stateMachine.ReusableData.CurrentWorldPosition = stateMachine.Koma.transform.position;
            stateMachine.ReusableData.CurrentShogiPosition = stateMachine.Koma.TargetShogiPosition;

            GameManager.Instance.ResetMovableGrid();
            stateMachine.ChangeState(stateMachine.IdlingState);
        }
    }
}
