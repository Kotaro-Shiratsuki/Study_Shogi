using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
using UnityEngine.InputSystem;
using System;

public class Player : MonoBehaviour
{
    [field: Header("Input")]
    public PlayerInputActions InputActions { get; private set; }
    public PlayerInputActions.PlayerActions Actions { get; private set; }


    [field: Header("Layers")]
    [field: SerializeField]
    public LayerMask GridLayer { get; private set; }

    [field: SerializeField]
    public LayerMask KomaLayer { get; private set; }

    [field: SerializeField]
    public LayerMask UiLayer { get; private set; }

    // public event Action OnClickEvent;

    #region Private Member
    private Vector2Int clickedPosition;
    #endregion


    #region Mono Methods
    private void Awake()
    {
        // InputSystemの初期化
        InputActions = new PlayerInputActions();
        Actions = InputActions.Player;
    }

    private void Start()
    {
        Initialize();
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
    private void Initialize()
    {
        clickedPosition = new Vector2Int(-1, -1);
    }

    private void AddInputActionCallBacks()
    {
        Actions.Fire.started += OnMouseLeftButtonClicked;
    }

    private void RemoveInputActionCallBack()
    {
        Actions.Fire.started -= OnMouseLeftButtonClicked;
    }

    private void OnMouseLeftButtonClicked(InputAction.CallbackContext context)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, GridLayer))
        {
            var grid = hit.collider.gameObject.GetComponent<ShogiGrid>();
            UpdateClickedPosition(grid.GetShogiPosition());

#if UNITY_EDITOR
            // デバッグ用
            Debug.Log("clicked position : " + clickedPosition);
            Debug.Log("Grid data Region : " + grid.GridData.Region + ", State : " + grid.GridData.State);
            if (GameManager.Instance.KomaDictionary.TryGetValueFirstKey(clickedPosition, out var koma))
            {
                Debug.Log("Koma data ID : " + koma.Data.Status.KomaID + ", HP : " + koma.GetCurrentStatus().CurrentHitPoint + ", AP : " + koma.GetCurrentStatus().CurrentAttackPoint);
            }
#endif
        }
    }

    private void UpdateClickedPosition(Vector2Int position)
    {
        if (position == clickedPosition)
        {
            return;
        }

        if(CheckMovableGrid(position))
        {
            Move(position);
            clickedPosition = position;
        }
        else if(CheckRuledGrid(position))
        {
            // 移動可能マスの更新
            GameManager.Instance.ResetMovableGrid();
            GameManager.Instance.UpdateMovableGrid(position);

            // とりあえず選択状態を外す
            OutKomaClicked();
            clickedPosition = position;
            OnKomaClicked();
        }
        else
        {
            // 移動可能マスの更新
            GameManager.Instance.ResetMovableGrid();

            // とりあえず選択状態を外す
            OutKomaClicked();
            clickedPosition = position;
            OnKomaClicked();
        }

           // クリックイベントを発火する
           //OnClickEvent.Invoke();
    }

    private bool CheckMovableGrid(Vector2Int position)
    {
        return GameManager.Instance.GridDictionary.GetValueByFirstKey(position).GridData.State == GridState.Movable;
    }

    /// <summary>
    /// マス目の上に駒が乗っているならTrue、空きマスならFalseを返す
    /// </summary>
    private bool CheckRuledGrid(Vector2Int position)
    {
        return GameManager.Instance.KomaDictionary.ContainsFirstKey(position);
    }

    private void Move(Vector2Int target)
    {
        Debug.Log("Move");

        if(GameManager.Instance.KomaDictionary.TryGetValueFirstKey(clickedPosition, out var koma))
        {
            koma.UpdateTargetPosition(GameManager.Instance.GridDictionary.GetValueByFirstKey(target).GridData.WorldPosition, target);
            koma.Move();
        }
    }

    private void OnKomaClicked()
    {
        Koma clicked;

        if (GameManager.Instance.KomaDictionary.TryGetValueFirstKey(clickedPosition, out clicked))
        {
            clicked.OnClicked();
        }
    }

    private void OutKomaClicked()
    {
        Koma clicked;

        if (GameManager.Instance.KomaDictionary.TryGetValueFirstKey(clickedPosition, out clicked))
        {
            clicked.RemoveSelected();
        }
    }
    #endregion
}
