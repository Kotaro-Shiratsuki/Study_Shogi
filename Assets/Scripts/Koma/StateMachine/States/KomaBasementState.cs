using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KomaBasementState : IState
{
    protected KomaMovementStateMachine stateMachine;
    protected KomaMovementData movementData;
    protected KomaStatusData statusData;
    protected KomaSkillsData skillsData;

    public KomaBasementState(KomaMovementStateMachine komaMovementStateMachine)
    {
        // コンストラクタ内で各種データをつなぎ合わせる
        stateMachine = komaMovementStateMachine;
        movementData = stateMachine.Koma.Data.MovementData;
        statusData = stateMachine.Koma.Data.Status;
        skillsData = stateMachine.Koma.Data.SkillData;

        InitializeData();
    }

    /// <summary>
    /// 初期化処理。必要に応じて追記すること。
    /// </summary>
    private void InitializeData()
    {
        SetBaseStatusData();
    }


    #region IState Methods
    // インターフェースで定義したメソッドを仮想関数として再定義

    public virtual void OnEnter()
    {
        // Stateに入る度に、現在どのステートにいるのかをログ表示
        Debug.Log("State" + GetType().Name);
    }

    public virtual void OnExit()
    {
        
    }

    public virtual void HandleInput()
    {
        ReadInput();
    }

    public virtual void NormalUpdate()
    {
        
    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void OnAnimationEnterEvent()
    {
        
    }

    public virtual void OnAnimationExitEvent()
    {
        
    }

    public virtual void OnAnimationTransitionEvent()
    {
       
    }
    #endregion

    #region Main Methods
    /// <summary>
    /// 入力の読み取り
    /// </summary>
    private void ReadInput()
    {

    }

    /// <summary>
    /// 駒が保持するステータスデータの初期化
    /// </summary>
    private void SetBaseStatusData()
    {
        KomaStateReusableData data = stateMachine.ReusableData;

        data.CurrentLevel = 1;
        data.CurrentHitPoint = statusData.StatusTable[0].HitPoints;
        data.CurrentAttackPoint = statusData.StatusTable[0].AttackPoints;
        data.CurrentExp = statusData.StatusTable[0].Exp;
        data.ExpToNextLevel = statusData.StatusTable[0].ExpToNextLevel;
        data.AmountOfExp = 0;
    }

    /// <summary>
    /// Status更新処理
    /// </summary>
    private void UpdateStatus(int currentLevel)
    {
        stateMachine.ReusableData.CurrentHitPoint = CorrectHitPoint(currentLevel);
        stateMachine.ReusableData.CurrentAttackPoint = statusData.StatusTable[currentLevel].AttackPoints;
        stateMachine.ReusableData.CurrentExp = statusData.StatusTable[currentLevel ].Exp;
        stateMachine.ReusableData.ExpToNextLevel += statusData.StatusTable[currentLevel].ExpToNextLevel;
    }

    /// <summary>
    /// HP増加の調整処理
    /// </summary>
    private int CorrectHitPoint(int currentLevel)
    {
        int currentIndex = currentLevel - 1;
        int currentMax = statusData.StatusTable[currentIndex].HitPoints;
        int nextMax = statusData.StatusTable[currentIndex + 1].HitPoints;

        // HP増加の実数値
        int correctHP = nextMax - currentMax;

        // 最大HPを超える場合は最大HPを、そうでないなら増加実数値を現在HPに足した値を返す
        if(stateMachine.ReusableData.CurrentHitPoint + correctHP >= nextMax)
        {
            return nextMax;
        }
        else
        {
            return stateMachine.ReusableData.CurrentHitPoint + correctHP;
        }
    }
    #endregion

    #region Reusable Methods
    // 複数のステートで使いまわす処理

    /// <summary>
    /// アニメーション再生命令
    /// </summary>
    /// <param name="animationHash"></param>
    protected void StartAnimation(int animationHash)
    {
        stateMachine.Koma.Animator.SetBool(animationHash, true);
    }

    /// <summary>
    /// アニメーション停止命令
    /// </summary>
    /// <param name="animationHash"></param>
    protected void StopAnimation(int animationHash)
    {
        stateMachine.Koma.Animator.SetBool(animationHash, false);
    }

    /// <summary>
    /// レベルアップ処理
    /// </summary>
    protected void AddLevel()
    {
        if(stateMachine.ReusableData.CurrentLevel < statusData.MaxLevel)
        {
            UpdateStatus(stateMachine.ReusableData.CurrentLevel);
            stateMachine.ReusableData.CurrentLevel += 1;
        }
    }
    #endregion
}
