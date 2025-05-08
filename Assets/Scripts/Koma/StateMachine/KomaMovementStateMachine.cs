public class KomaMovementStateMachine : StateMachine
{
    public Koma Koma { get; }
    public KomaStateReusableData ReusableData { get; }
    public CurrentStatus CurrentStatus { get; private set; }

    // Fielded State
    public KomaIdlingState IdlingState { get; }
    public KomaSelectedState SelectedState { get; }
    public KomaWaitingState WaitingState { get; }
    public KomaMovingState MovingState { get; }
    public KomaMoveWithEvState MoveWithEvState { get; }
    public KomaAttackingState AttackingState { get; }
    public KomaAttackWithEvState AttackWithEvState { get; }
    public KomaDefeatedState DefeatedState { get; }
    public KomaStoppingState StoppingState { get; }
    public KomaDamagedState DamagedState { get; }

    // Cuptured State
    public KomaMoveToSideState MoveToSideState { get; }
    public KomaStopOnSideState StopOnSideState { get; }
    public KomaIdleOnSideState IdleOnSideState { get; }
    public KomaSelectOnSideState SelectOnSideState { get; }
    public KomaWaitOnSideState WaitOnSideState { get; }
    public KomaMoveToBoardState MoveToBoardState { get; }
    public KomaStopOnBoardState StopOnBoardState { get; }

    public KomaMovementStateMachine(Koma koma)
    {
        Koma = koma;
        ReusableData = new KomaStateReusableData();
        CurrentStatus = new CurrentStatus();
        SetCurrentStatus();

        // States initialization
        IdlingState = new KomaIdlingState(this);
        SelectedState = new KomaSelectedState(this);
        WaitingState = new KomaWaitingState(this);
        MovingState = new KomaMovingState(this);
        MoveWithEvState = new KomaMoveWithEvState(this);
        AttackingState = new KomaAttackingState(this);
        AttackWithEvState = new KomaAttackWithEvState(this);
        DefeatedState = new KomaDefeatedState(this);
        StoppingState = new KomaStoppingState(this);
        DamagedState = new KomaDamagedState(this);
        MoveToSideState = new KomaMoveToSideState(this);
        StopOnSideState = new KomaStopOnSideState(this);
        IdleOnSideState = new KomaIdleOnSideState(this);
        SelectOnSideState = new KomaSelectOnSideState(this);
        WaitOnSideState = new KomaWaitOnSideState(this);
        MoveToBoardState = new KomaMoveToBoardState(this);
        StopOnBoardState = new KomaStopOnBoardState(this);
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