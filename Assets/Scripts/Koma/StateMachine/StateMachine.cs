using UnityEngine;

public abstract class StateMachine
{
    protected IState currentState;

    /// <summary>
    /// State切替処理。
    /// </summary>
    /// <param name="state"></param>
    public void ChangeState(IState state)
    {
        currentState?.OnExit();
        currentState = state;
        currentState?.OnEnter();
    }

    /// <summary>
    /// 毎フレームUpdateより先に実行される処理
    /// </summary>
    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    /// <summary>
    /// 毎フレーム実行される処理
    /// </summary>
    public void NormalUpdate()
    {
        currentState?.NormalUpdate();
    }

    /// <summary>
    /// Time.FixedDeltaTime 秒に1回実行される処理。物理演算は必ずこの中で行うこと。
    /// </summary>
    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }

    /// <summary>
    /// アニメーション再生時に発火するイベント。アニメーターで設定する。
    /// </summary>
    public void OnAnimationEnterEvent()
    {
        currentState?.OnAnimationEnterEvent();
    }

    /// <summary>
    /// アニメーション停止時に発火するイベント。アニメーターで設定する。
    /// </summary>
    public void OnAnimationExitEvent()
    {
        currentState?.OnAnimationExitEvent();
    }

    /// <summary>
    /// アニメーション遷移時に発火するイベント。アニメーターで設定する。
    /// </summary>
    public void OnAnimationTransitionEvent()
    {
        currentState?.OnAnimationTransitionEvent();
    }
}
