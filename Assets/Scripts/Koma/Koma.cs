using UnityEngine;
using UnityEngine.Events;

public class Koma : MonoBehaviour
{
    [field: Header("Reference")]
    [field: SerializeField]
    public KomaSO Data {  get; private set; }

    [field: Header("Owner")]
    [field: SerializeField]
    public Owner Owner { get; private set; }

    [field: Header("Animation")]
    [field: SerializeField]
    public KomaAnimationData AnimationData { get; private set; }

    // Move target position
    public Vector3 TargetWorldPosition { get; private set; }
    public Vector2Int TargetShogiPosition { get; private set; }

    // Animator
    public Animator Animator { get; private set; }

    public UnityAction RemoveUICallbacks;

    // State machine
    private KomaMovementStateMachine movementStateMachine;

    #region Mono Method
    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        AnimationData.Initialize();

        movementStateMachine = new KomaMovementStateMachine(this);

        movementStateMachine.ReusableData.MoveSpeedModifier = 0f;
        movementStateMachine.ReusableData.JumpHeightModifier = 0f;
    }

    private void Start()
    {
        movementStateMachine.ChangeState(movementStateMachine.IdlingState);
    }

    private void Update()
    {
        movementStateMachine.NormalUpdate();
    }
    #endregion

    #region Get Parameter Method
    /// <summary>
    /// Return koma's current status parameters.
    /// </summary>
    public CurrentStatus GetCurrentStatus()
    {
        return movementStateMachine.GetCurrentStatus();
    }

    public bool GetIsEvolved()
    {
        return movementStateMachine.ReusableData.IsEvolved;
    }

    public bool GetCanEvolution()
    {
        if(!Data.Status.CanEvolution)
        {
            return false;
        }

        return movementStateMachine.ReusableData.IsEvolved ? false : true;
    }

    public Vector2Int GetCurrentShogiPosition()
    {
        return movementStateMachine.ReusableData.CurrentShogiPosition;
    }
    #endregion

    #region Set Or Update Parameter Method
    /// <summary>
    /// Set initial position.
    /// </summary>
    public void InitKomaPosition(Vector3 InitWorldPosition, Vector2Int initShogiPosition)
    {
        movementStateMachine.ReusableData.CurrentWorldPosition = InitWorldPosition;
        movementStateMachine.ReusableData.CurrentShogiPosition = initShogiPosition;
    }

    /// <summary>
    /// Set initial owner.
    /// </summary>
    public void SetInitialOwner(bool isPlayer)
    {
        Owner = isPlayer ? Owner.Player : Owner.Enemy;
    }

    /// <summary>
    /// Update world position.
    /// </summary>
    public void UpdateWorldPosition(Vector3 newPosition)
    {
        movementStateMachine.ReusableData.CurrentWorldPosition = newPosition;
    }

    /// <summary>
    /// Update move target position.
    /// </summary>
    public void UpdateTargetPosition(Vector3 targetPosition, Vector2Int shogiPosition)
    {
        TargetWorldPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        TargetShogiPosition = shogiPosition;
    }
    #endregion

    #region Switching State On Action Method
    public void OnClicked()
    {
        movementStateMachine.ChangeState(movementStateMachine.SelectedState);
    }

    public void RemoveSelected()
    {
        movementStateMachine.ChangeState(movementStateMachine.IdlingState);
    }

    public void RemoveSelectedOnSide()
    {
        movementStateMachine.ChangeState(movementStateMachine.IdleOnSideState);
    }

    public void Wait()
    {
        movementStateMachine.ChangeState(movementStateMachine.WaitingState);
    }

    public void Move()
    {
        movementStateMachine.ChangeState(movementStateMachine.MovingState);
    }

    public void MoveWithEv()
    {
        movementStateMachine.ChangeState(movementStateMachine.MoveWithEvState);
    }

    public void WaitOnSide()
    {
        movementStateMachine.ChangeState(movementStateMachine.WaitOnSideState);
    }

    public void MoveOnSide()
    {
        movementStateMachine.ChangeState(movementStateMachine.MoveToBoardState);
    }

    public void Attack()
    {
        movementStateMachine.ChangeState(movementStateMachine.AttackingState);
    }

    public void AttackWithEv()
    {
        movementStateMachine.ChangeState(movementStateMachine.AttackWithEvState);
    }

    public void OnAttacked()
    {
        movementStateMachine.ChangeState(movementStateMachine.DamagedState);
    }

    public void SelectedOnSide()
    {
        movementStateMachine.ChangeState(movementStateMachine.SelectOnSideState);
    }

    /// <summary>
    /// When koma is defeated, switching owner and move opposite side reserv.
    /// </summary>
    public void OnDefeated()
    {
        var target = Owner == Owner.Enemy ?
            GameManager.Instance.FriendSideBoard.GetTargetPosition(Data.Status.KomaID)
            : GameManager.Instance.EnemySideBoard.GetTargetPosition(Data.Status.KomaID);

        TargetWorldPosition = new Vector3(target.x, movementStateMachine.ReusableData.CurrentWorldPosition.y, target.z);
        SwitchOwner();

        movementStateMachine.ChangeState(movementStateMachine.MoveToSideState);
    }
    #endregion

    #region Animation Event
    public void OnMovementStateAnimationEnterEvent()
    {
        movementStateMachine.OnAnimationEnterEvent();
    }

    public void OnMovementStateAnimationExitEvent()
    {
        movementStateMachine.OnAnimationExitEvent();
    }

    public void OnMovementStateAnimationTransitionEvent()
    {
        movementStateMachine.OnAnimationTransitionEvent();
    }
    #endregion

    #region UI Event
    /// <summary>
    /// When the action button is clicked, the state of the Koma is transitioned by looking at the state of the grid and whether it is evolvable.
    /// </summary>
    public void OnActionButtonClicked(GridState target, bool isEvo = false)
    {
        switch(target)
        {
            case GridState.Movable:
                Move();
                break;

            case GridState.MovableWithEv:
                if (Data.Status.CanEvolution && !movementStateMachine.ReusableData.IsEvolved && isEvo)
                {
                    MoveWithEv();
                    movementStateMachine.ReusableData.IsEvolved = true;
                }
                else
                {
                    Move();
                }
                    break;

            case GridState.Attackable:
                Attack();
                break;

            case GridState.AttackableWithEv:
                if (Data.Status.CanEvolution && !movementStateMachine.ReusableData.IsEvolved && isEvo)
                {
                    AttackWithEv();
                    movementStateMachine.ReusableData.IsEvolved = true;
                }
                else
                {
                    Attack();
                }
                break;

            default:
                break;
        }
    }

    public void RemoveUIListeners()
    {
        RemoveUICallbacks.Invoke();
    }
    #endregion

    #region Private Method
    private void SwitchOwner()
    {
        Owner = Owner == Owner.Player ? Owner.Enemy : Owner.Player;
    }
    #endregion
}

public class CurrentStatus
{
    public int CurrentLevel { get; set; }
    public int CurrentHitPoint { get; set; }
    public int CurrentAttackPoint { get; set; }
    public int CurrentExp { get; set; }
    public int AmountOfExp { get; set; }
    public int ExpToNextLevel { get; set; }
    public bool IsEvolved { get; set; }
}

public enum Owner
{
    None,
    Player,
    Enemy,
    Size
}

