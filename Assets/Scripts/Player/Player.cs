using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class for receiving player input and issuing commands to shogi's koma.
/// </summary>
public class Player : MonoBehaviour
{
    public enum Side
    {
        None,
        Friend,
        Enemy,
        Size
    }

    [field: SerializeField]
    public Side CurrentSide {  get; private set; }

    [field: Header("Input")]
    public PlayerInputActions InputActions { get; private set; }
    public PlayerInputActions.PlayerActions Actions { get; private set; }

    [field: Header("Layers")]
    [field: SerializeField]
    public LayerMask GridLayer { get; private set; }

    [field: SerializeField]
    public LayerMask AnchorLayer { get; private set; }


    [field: SerializeField]
    public float DistanceOfRay { get; private set; } = 50.0f;

    public GameManager Manager {  get; private set; }
    private CanvasController canvas;
    private PlayerOnClickMovements moves;

    #region Mono Methods
    private void Awake()
    {
        // Initialize Input System
        InputActions = new PlayerInputActions();
        Actions = InputActions.Player;
    }

    private void Start()
    {
        Manager = GameManager.Instance;
        canvas = Manager.Canvas;
        moves = new PlayerOnClickMovements(this);
    }

    private void OnEnable()
    {
        InputActions.Enable();

        Debug.Log("Add Call Backs");
        AddInputActionCallBacks();
    }

    private void OnDisable()
    {
        InputActions.Disable();

        Debug.Log("Remove Call Backs");
        RemoveInputActionCallBack();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Register input call backs.
    /// </summary>
    private void AddInputActionCallBacks()
    {
        Actions.Fire.started += OnMouseLeftButtonClicked;
    }

    /// <summary>
    /// Remove registered call backs.
    /// </summary>
    private void RemoveInputActionCallBack()
    {
        Actions.Fire.started -= OnMouseLeftButtonClicked;
    }

    /// <summary>
    /// The event when player click a mouse left button.
    /// </summary>
    /// <param name="context"></param>
    private void OnMouseLeftButtonClicked(InputAction.CallbackContext context)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        moves.CastRayOnClicked(mousePos, ray);
    }

    /// <summary>
    /// Cancel button behavior.
    /// </summary>
    private void OnCancelButtonClicked()
    {
        moves.OnCancelButtonClicked();
    }
    
    /// <summary>
    /// Action button behavior.
    /// </summary>
    private void OnActionButtonClicked()
    {
        moves.OnActionButtonClicked();
        canvas.DisableActionButton();
    }
    #endregion

    #region Internal Methods

    /// <summary>
    /// Show action buttons and register listeners.
    /// </summary>
    internal void ShowActionButtons()
    {
        canvas.ShowButtons(moves.ShowActionButtons());

        canvas.ActionButtonsPanel.ActionButton.onClick.AddListener(OnActionButtonClicked);
        canvas.ActionButtonsPanel.CancelButton.onClick.AddListener(OnCancelButtonClicked);
    }

    internal void SwitchActionButtons(GridState state)
    {
        canvas.SwitchActionButton(state);
    }

    internal void HideUiOnWaitCanceling()
    {
        Manager.CancelWaiting();
    }

    internal void WaitOnSideBoard()
    {
       moves.WaitOnSideBoard();
    }

    /// <summary>
    /// Get is evolution toggles's value.
    /// </summary>
    /// <returns></returns>
    internal bool IsEvolution()
    {
        return canvas.ActionButtonsPanel.IsEvolution.isOn;
    }

    /// <summary>
    /// Remove koma's clicked state.
    /// </summary>
    internal void ResetClickedKoma()
    {
        moves.ResetClickedKoma();
    }

    /// <summary>
    /// Remove anchor's selected state.
    /// </summary>
    internal void ResetSelectedAnchor()
    {
        moves.ResetSelectedAnchor();
    }
    #endregion
}
