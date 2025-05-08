using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 駒ステートマシーンの本体
/// </summary>
public class KomaMovementStateMachine : StateMachine
{
    public Koma Koma { get; }
    public KomaStateReusableData ReusableData { get; }
    public CurrentStatus CurrentStatus { get; private set; }

    // Stateを増やすときはここに追加する
    public KomaIdlingState IdlingState { get; }
    public KomaSelectedState SelectedState { get; }
    public KomaMovingState MovingState { get; }

    public KomaMovementStateMachine(Koma koma)
    {
        Koma = koma;
        ReusableData = new KomaStateReusableData();
        CurrentStatus = new CurrentStatus();
        SetCurrentStatus();

        // Stateの初期化
        IdlingState = new KomaIdlingState(this);
        SelectedState = new KomaSelectedState(this);
        MovingState = new KomaMovingState(this);
    }

    #region Main Methods
    private void SetCurrentStatus()
    {
        CurrentStatus.CurrentLevel = ReusableData.CurrentLevel;
        CurrentStatus.CurrentHitPoint = ReusableData.CurrentHitPoint;
        CurrentStatus.CurrentAttackPoint = ReusableData.CurrentAttackPoint;
        CurrentStatus.CurrentExp = ReusableData.CurrentExp;
        CurrentStatus.AmountOfExp = ReusableData.AmountOfExp;
        CurrentStatus.ExpToNextLevel = ReusableData.ExpToNextLevel;
        CurrentStatus.IsEvolved = ReusableData.IsEvolved;
    }

    public CurrentStatus GetCurrentStatus()
    {
        SetCurrentStatus();

        return CurrentStatus;
    }
    #endregion
}