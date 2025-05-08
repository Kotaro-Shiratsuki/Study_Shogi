using UnityEngine;

[System.Serializable]
public class KomaAnimationData
{
    [Header("Fielded Group Parameter Names")]
    [SerializeField] private string fieldedParameterName = "Fielded";
    [SerializeField] private string idlingParameterName = "Idling";
    [SerializeField] private string selectedParameterName = "Selected";
    [SerializeField] private string waitingParameterName = "Waiting";
    [SerializeField] private string movingParameterName = "Moving";
    [SerializeField] private string stoppingParameterName = "Stopping";
    [SerializeField] private string attackingParameterName = "Attacking";
    [SerializeField] private string damagedParameterName = "Damaged";
    [SerializeField] private string defeatedParameterName = "Defeated";

    [Header("Cuptured Group Parameter Names")]
    [SerializeField] private string cupturedParameterName = "Cuptured";
    [SerializeField] private string idleOnSideParameterName = "IdleOnSide";
    [SerializeField] private string moveToSideParameterName = "MoveToSide";
    [SerializeField] private string selectOnSideParameterName = "SelectOnSide";
    [SerializeField] private string stopOnSideParameterName = "StopOnSide";
    [SerializeField] private string moveToBoardParameterName = "MoveToBoard";
    [SerializeField] private string stopOnBoardParameterName = "StopOnBoard";


    public int FieldedParameterHash {  get; private set; }
    public int IdlingParameterHash {  get; private set; }
    public int SelectedParameterHash {  get; private set; }
    public int WaitingParameterHash { get; private set; }
    public int MovingParameterHash { get; private set ; }
    public int StoppingParameterHash { get; private set ; }
    public int AttackingParameterHash { get; private set ; }
    public int DamagedParameterHash { get; private set ; }
    public int DefeatedParameterHash { get; private set ; }
    public int CupturedParameterHash {  get; private set ; }
    public int IdleOnSideParameterHash { get; private set ; }
    public int MoveToSideParameterHash { get; private set ; }
    public int SelectOnSideParameterHash { get; private set ; }
    public int StopOnSideParameterHash { get; private set; }
    public int MoveToBoardParameterHash { get; private set; }
    public int StopOnBoardParameterHash { get; private set; }


    public void Initialize()
    {
        // Convert animtor param name to hash. (More faster than use string.)
        FieldedParameterHash = Animator.StringToHash(fieldedParameterName);
        IdlingParameterHash = Animator.StringToHash(idlingParameterName);
        SelectedParameterHash = Animator.StringToHash(selectedParameterName);
        WaitingParameterHash = Animator.StringToHash(waitingParameterName);
        MovingParameterHash = Animator.StringToHash(movingParameterName);
        StoppingParameterHash = Animator.StringToHash(stoppingParameterName);
        AttackingParameterHash = Animator.StringToHash(attackingParameterName);
        DamagedParameterHash = Animator.StringToHash(damagedParameterName);
        DefeatedParameterHash = Animator.StringToHash(defeatedParameterName);
        CupturedParameterHash = Animator.StringToHash(cupturedParameterName);
        IdleOnSideParameterHash = Animator.StringToHash(idleOnSideParameterName);
        MoveToSideParameterHash = Animator.StringToHash(moveToSideParameterName);
        SelectOnSideParameterHash = Animator.StringToHash(selectOnSideParameterName);
        StopOnSideParameterHash = Animator.StringToHash(stopOnSideParameterName);
        MoveToBoardParameterHash = Animator.StringToHash(moveToBoardParameterName);
        StopOnBoardParameterHash = Animator.StringToHash(stopOnBoardParameterName);
    }
}