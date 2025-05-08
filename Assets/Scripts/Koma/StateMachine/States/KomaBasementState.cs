using UnityEngine;

public class KomaBasementState : IState
{
    protected KomaMovementStateMachine stateMachine;
    protected KomaMovementData movementData;
    protected KomaStatusData statusData;
    protected KomaSkillsData skillsData;
    protected KomaSettingsData settingsData;

    private Vector3 move = Vector3.zero;
    private Vector3 direction;
    private float distance;
    private float maxDistance;
    private float moveScaler;
    private float jumpHeightModifier;
    private float floorHeight;
    private Quaternion start;
    private Quaternion targetRotationOnSide;
    private Quaternion targetRotationOnEvolved;

    public KomaBasementState(KomaMovementStateMachine komaMovementStateMachine)
    {
        stateMachine = komaMovementStateMachine;
        movementData = stateMachine.Koma.Data.MovementData;
        statusData = stateMachine.Koma.Data.Status;
        skillsData = stateMachine.Koma.Data.SkillData;
        settingsData = stateMachine.Koma.Data.SettingsData;

        InitializeData();
    }

    private void InitializeData()
    {
        SetBaseStatusData();
    }


    #region IState Methods
    // Redefine methods defined in interfaces as virtual.

    public virtual void OnEnter()
    {
        // Log display of which state you are currently in each time you enter a State
        Debug.Log("State : " + GetType().Name);
    }

    public virtual void OnExit()
    {
        
    }

    public virtual void HandleInput()
    {

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
    /// Process to update status.
    /// </summary>
    private void UpdateStatus(int currentLevel)
    {
        stateMachine.ReusableData.CurrentHitPoint = CorrectHitPoint(currentLevel);
        stateMachine.ReusableData.CurrentAttackPoint = statusData.StatusTable[currentLevel].AttackPoints;
        stateMachine.ReusableData.CurrentExp = statusData.StatusTable[currentLevel ].Exp;
        stateMachine.ReusableData.ExpToNextLevel += statusData.StatusTable[currentLevel].ExpToNextLevel;
    }

    /// <summary>
    /// HP adjustment process at level up.
    /// </summary>
    private int CorrectHitPoint(int currentLevel)
    {
        int currentIndex = currentLevel - 1;
        int currentMax = statusData.StatusTable[currentIndex].HitPoints;
        int nextMax = statusData.StatusTable[currentIndex + 1].HitPoints;

        int correctHP = nextMax - currentMax;

        // Returns the maximum HP if it exceeds the maximum HP, otherwise returns the real value of the increase plus the current HP.
        if (stateMachine.ReusableData.CurrentHitPoint + correctHP >= nextMax)
        {
            return nextMax;
        }
        else
        {
            return stateMachine.ReusableData.CurrentHitPoint + correctHP;
        }
    }

    /// <summary>
    /// Parabolic trajectory movement process.
    /// </summary>
    private void ParabolaMovement()
    {
        move = direction * stateMachine.ReusableData.MoveSpeedModifier * Time.deltaTime;

        Vector3 movedPosition = stateMachine.Koma.transform.position + move;
        movedPosition.y = CalcParabolaHeight();

        stateMachine.Koma.transform.position = movedPosition;
        distance -= move.magnitude;
    }

    /// <summary>
    /// Parabolic trajectory movement process with evolution.
    /// </summary>
    private void ParabolaMoveWithEv()
    {
        move = direction * stateMachine.ReusableData.MoveSpeedModifier * Time.deltaTime;
        Vector3 movedPosition = stateMachine.Koma.transform.position + move;
        movedPosition.y = CalcParabolaHeight();

        var rotation = Quaternion.Slerp(start, targetRotationOnEvolved, DistanceScaler(moveScaler));

        stateMachine.Koma.transform.position = movedPosition;
        stateMachine.Koma.transform.rotation = rotation;
        distance -= move.magnitude;
    }

    /// <summary>
    /// Parabolic trajectory movement process at cuptured.
    /// </summary>
    private void CupturedMovement()
    {
        move = direction * stateMachine.ReusableData.MoveSpeedModifier * Time.deltaTime;
        Vector3 movedPosition = stateMachine.Koma.transform.position + move;
        movedPosition.y = CalcParabolaHeight();

        var rotation = Quaternion.Slerp(start, targetRotationOnSide, DistanceScaler(moveScaler));

        stateMachine.Koma.transform.position = movedPosition;
        stateMachine.Koma.transform.rotation = rotation;

        distance -= move.magnitude;
    }

    private float CalcParabolaHeight()
    {
        // Calc the parabola of y = -ax^2 + ax 
        float height;
        moveScaler += move.magnitude;
        height = -(jumpHeightModifier * Mathf.Pow(DistanceScaler(moveScaler), 2)) + (jumpHeightModifier * DistanceScaler(moveScaler));

        return height + floorHeight;
    }

    private float DistanceScaler(float move)
    {
        return moveScaler / maxDistance;
    }
    #endregion

    #region Reusable Methods
    protected void SetBaseStatusData()
    {
        stateMachine.ReusableData.CurrentLevel = 1;
        stateMachine.ReusableData.CurrentHitPoint = statusData.StatusTable[0].HitPoints;
        stateMachine.ReusableData.CurrentAttackPoint = statusData.StatusTable[0].AttackPoints;
        stateMachine.ReusableData.CurrentExp = statusData.StatusTable[0].Exp;
        stateMachine.ReusableData.ExpToNextLevel = statusData.StatusTable[0].ExpToNextLevel;
        stateMachine.ReusableData.AmountOfExp = 0;
        stateMachine.ReusableData.IsEvolved = false;
    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.Koma.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.Koma.Animator.SetBool(animationHash, false);
    }

    protected void AddLevel()
    {
        if(stateMachine.ReusableData.CurrentLevel < statusData.MaxLevel)
        {
            UpdateStatus(stateMachine.ReusableData.CurrentLevel);
            stateMachine.ReusableData.CurrentLevel += 1;
        }
    }

    protected void Move(float minimumDistance = 0.05f)
    {
        if(distance > minimumDistance)
        {
            ParabolaMovement();
        }
        else
        {
            stateMachine.Koma.transform.position = stateMachine.ReusableData.MoveTargetPosition;
            stateMachine.ReusableData.CurrentWorldPosition = stateMachine.Koma.transform.position;
            stateMachine.ReusableData.CurrentShogiPosition = stateMachine.Koma.TargetShogiPosition;

            GameManager.Instance.ResetGridState(stateMachine.Koma.Owner);
            stateMachine.ChangeState(stateMachine.StoppingState);
        }
    }

    protected void MoveToSide(float minimumDistance = 0.05f)
    {
        if (distance > minimumDistance)
        {
            CupturedMovement();
        }
        else
        {
            stateMachine.Koma.transform.position = stateMachine.ReusableData.MoveTargetPosition;
            stateMachine.Koma.transform.rotation = targetRotationOnSide;
            stateMachine.ReusableData.CurrentWorldPosition = stateMachine.Koma.transform.position;
            stateMachine.ReusableData.CurrentShogiPosition = new Vector2Int(-1, -1);

            GameManager.Instance.ResetGridState(stateMachine.Koma.Owner);
            stateMachine.ChangeState(stateMachine.StopOnSideState);
        }
    }

    protected void MoveToBoard(float minimumDistance = 0.05f)
    {
        if (distance > minimumDistance)
        {
            ParabolaMovement();
        }
        else
        {
            stateMachine.Koma.transform.position = stateMachine.ReusableData.MoveTargetPosition;
            stateMachine.ReusableData.CurrentWorldPosition = stateMachine.Koma.transform.position;
            stateMachine.ReusableData.CurrentShogiPosition = stateMachine.Koma.TargetShogiPosition;

            GameManager.Instance.ResetGridState(stateMachine.Koma.Owner);
            stateMachine.ChangeState(stateMachine.StopOnBoardState);
        }
    }

    protected void MoveWithEvolution(float minimumDistance = 0.05f)
    {
        if (distance > minimumDistance)
        {
            ParabolaMoveWithEv();
        }
        else
        {
            stateMachine.Koma.transform.position = stateMachine.ReusableData.MoveTargetPosition;
            stateMachine.Koma.transform.rotation = targetRotationOnEvolved;
            stateMachine.ReusableData.CurrentWorldPosition = stateMachine.Koma.transform.position;
            stateMachine.ReusableData.CurrentShogiPosition = stateMachine.Koma.TargetShogiPosition;

            GameManager.Instance.ResetGridState(stateMachine.Koma.Owner);
            stateMachine.ChangeState(stateMachine.StoppingState);
        }
    }

    protected void InitMovementParameter()
    {
        // Initialize movement data
        float multi = stateMachine.ReusableData.IsEvolved ? -180f : 0f;
        direction = stateMachine.ReusableData.MoveTargetPosition - stateMachine.ReusableData.CurrentWorldPosition;
        maxDistance = direction.magnitude;
        distance = maxDistance;
        direction = Vector3.Normalize(direction);
        moveScaler = 0f;
        start = stateMachine.Koma.transform.rotation;
        targetRotationOnSide = start * Quaternion.AngleAxis(-180f, Vector3.up) * Quaternion.AngleAxis(multi, Vector3.forward);
        targetRotationOnEvolved = start * Quaternion.AngleAxis(-180f, Vector3.forward);

        // Initialize jumping data
        // 1/4 of the coefficient of x is the vertex, so multiply by 4
        jumpHeightModifier = stateMachine.ReusableData.JumpHeightModifier * 4.0f;
        floorHeight = stateMachine.ReusableData.CurrentWorldPosition.y;
    }
    #endregion
}
