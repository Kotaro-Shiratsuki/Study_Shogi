using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonPanel : MonoBehaviour
{
    [field: Header("Action Buttons")]
    [field: SerializeField]
    public Button ActionButton { get; private set; }

    [field: SerializeField]
    public Button CancelButton { get; private set; }

    [Header("Evo Toggle")]
    [SerializeField]
    public Toggle IsEvolution;

    private TMP_Text actionButtonText;
    private TMP_Text isEvoText;
    private Image toggleBox;

    public void Hide()
    {
        IsEvolution.isOn = false;
        gameObject.SetActive(false);
    }

   public void InitHide()
    {
        ActionButton.enabled = false;
        CancelButton.enabled = false;
        IsEvolution.enabled = false;

       gameObject.SetActive(false);
    }

    public void InitActionButton(Color color, string text)
    {
        actionButtonText = ActionButton.GetComponentInChildren<TMP_Text>();
        SetActionButtonsParam(color, text);
    }

    public void InitCancelButton(Color color, string text)
    {
        CancelButton.image.color = color;
        CancelButton.GetComponentInChildren<TMP_Text>().SetText(text);
    }

    public void InitIsEvo(string text, Color color)
    {
        isEvoText = IsEvolution.GetComponentInChildren<TMP_Text>();
        toggleBox = IsEvolution.transform.Find("Background").GetComponent<Image>();
        isEvoText.SetText(text);
        IsEvolution.isOn = false;

        SetIsEvoActive(false, color);
    }

    public void SetActionButtonsParam(Color color, string text)
    {
        ActionButton.image.color = color;
        actionButtonText.SetText(text);
    }

    public void RemoveAllListeners()
    {
        ActionButton.onClick.RemoveAllListeners();
        CancelButton.onClick.RemoveAllListeners();
    }

    public void SetIsEvoActive(bool isEv, Color color)
    {
        IsEvolution.enabled = isEv;
        isEvoText.color = color;
        toggleBox.color = color;
    }

    public void Enable(bool enabled)
    {
        ActionButton.enabled = enabled;
        CancelButton.enabled = enabled;
        IsEvolution.enabled = enabled;
        gameObject.SetActive(enabled);
    }


    #region As insurance, clear the listener when enabled, disabled, or destroyed.
    private void OnEnable()
    {
        Debug.Log("Action buttons are enebaled.");
        RemoveAllListeners();
    }

    private void OnDisable()
    {
        Debug.Log("Action buttons are disabled.");
        RemoveAllListeners();
    }

    private void OnDestroy()
    {
        Debug.Log("Action buttons are destroyed.");
        RemoveAllListeners();
    }
    #endregion
}
