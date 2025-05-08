using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private ActionButtonData actionButtonData;

    [field: Header("Action Button Group")]
    [field: SerializeField]
    public ActionButtonPanel ActionButtonsPanel {  get; private set; }

    private void Start()
    {
        Initialize();
    }

    public void SwitchActionButton(GridState target)
    {
        switch(target)
        {
            case GridState.Movable:
            case GridState.MovableWithEv:
                SetMoveActionButton();
                break;

            case GridState.Attackable:
            case GridState.AttackableWithEv:
                SetAttackActionButton();
                break;

            default:
                break;
        }
    }

    public void HideButtons()
    {
        ActionButtonsPanel.Hide();
    }

    public void ShowButtons(bool isEv = false)
    {
        Color col = isEv ? actionButtonData.ActiveColor : actionButtonData.DeactiveColor;
        EnableActionButton();
        ActionButtonsPanel.SetIsEvoActive(isEv, col);
    }

    public void DisableActionButton()
    {
        ActionButtonsPanel.Enable(false);
    }
    private void EnableActionButton()
    {
        ActionButtonsPanel.Enable(true);
    }

    private void Initialize()
    {
        InitActioButton();
        InitCancelButton();
        InitEvoToggle();

        ActionButtonsPanel.InitHide();
    }

    private void InitActioButton()
    {
        ActionButtonsPanel.InitActionButton(actionButtonData.MoveButtonColor, actionButtonData.MoveButtonText);
    }

    private void InitCancelButton()
    {
       ActionButtonsPanel.InitCancelButton(actionButtonData.CancelButtonColor, actionButtonData.CancelButtonText);
    }

    private void InitEvoToggle()
    {
        ActionButtonsPanel.InitIsEvo(actionButtonData.EvolutionToggleText, actionButtonData.DeactiveColor);
    }

    private void SetMoveActionButton()
    {
        ActionButtonsPanel.SetActionButtonsParam(actionButtonData.MoveButtonColor, actionButtonData.MoveButtonText);
    }

    private void SetAttackActionButton()
    {
        ActionButtonsPanel.SetActionButtonsParam(actionButtonData.AttackButtonColor, actionButtonData.AttackButtonText);
    }
}
