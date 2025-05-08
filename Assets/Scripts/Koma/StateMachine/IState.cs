using UnityEngine;

public interface IState
{
    /// <summary>
    /// Stateに入ってきて最初に実行される処理
    /// </summary>
    public void OnEnter();

    /// <summary>
    /// Stateを抜けるとき最後に実行される処理
    /// </summary>
    public void OnExit();

    /// <summary>
    /// 毎フレームUpdateより先に実行される処理
    /// </summary>
    public void HandleInput();

    /// <summary>
    /// 毎フレーム実行される処理
    /// </summary>
    public void NormalUpdate();

    /// <summary>
    /// Time.FixedDeltaTime 秒に1回実行される処理。物理演算は必ずこの中で行うこと。
    /// </summary>
    public void PhysicsUpdate();

    /// <summary>
    /// アニメーション再生時に発火するイベント。アニメーターで設定する。
    /// </summary>
    public void OnAnimationEnterEvent();

    /// <summary>
    /// アニメーション停止時に発火するイベント。アニメーターで設定する。
    /// </summary>
    public void OnAnimationExitEvent();

    /// <summary>
    /// アニメーション遷移時に発火するイベント。アニメーターで設定する。
    /// </summary>
    public void OnAnimationTransitionEvent();
}