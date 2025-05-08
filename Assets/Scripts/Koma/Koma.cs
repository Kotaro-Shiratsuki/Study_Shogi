using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public Vector3 TargetWorldPosition { get; private set; }
    public Vector2Int TargetShogiPosition { get; private set; }



    private KomaMovementStateMachine movementStateMachine;

    public Animator Animator { get; private set; }

    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        AnimationData.Initialize();

        movementStateMachine = new KomaMovementStateMachine(this);
    }

    private void Start()
    {
        movementStateMachine.ChangeState(movementStateMachine.IdlingState);
    }

    private void Update()
    {
        movementStateMachine.NormalUpdate();
    }

    public CurrentStatus GetCurrentStatus()
    {
        return movementStateMachine.GetCurrentStatus();
    }

    public void OnClicked()
    {
        movementStateMachine.ChangeState(movementStateMachine.SelectedState);
    }

    public void RemoveSelected()
    {
        movementStateMachine.ChangeState(movementStateMachine.IdlingState);
    }

    public void InitKomaPosition(Vector3 InitWorldPosition, Vector2Int initShogiPosition)
    {
        movementStateMachine.ReusableData.CurrentWorldPosition = InitWorldPosition;
        movementStateMachine.ReusableData.CurrentShogiPosition = initShogiPosition;
    }

    public void UpdateTargetPosition(Vector3 targetPosition, Vector2Int shogiPosition)
    {
        TargetWorldPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        TargetShogiPosition = shogiPosition;
    }

    public void Move()
    {
        movementStateMachine.ChangeState(movementStateMachine.MovingState);
    }

    public void ResetTargetPosition()
    {
        TargetWorldPosition = transform.position;
        TargetShogiPosition = new Vector2Int(-1, -1);
    }

    public void SetInitialOwner(bool isPlayer)
    {
        Owner = isPlayer ? Owner.Player : Owner.Enemy;
    }

    public void SwitchOwner()
    {
        Owner = Owner == Owner.Player ? Owner.Enemy : Owner.Player;
    }
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

