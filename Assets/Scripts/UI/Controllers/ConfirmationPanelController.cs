using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ConfirmationPanelController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _cancelButton;

    private System.Action _confirmAction;

    private void Start()
    {
        _panel.SetActive(false);
        _confirmButton.onClick.AddListener(OnConfirm);
        _cancelButton.onClick.AddListener(OnCancel);
    }

    public void ShowConfirmation(string message, System.Action confirmAction)
    {
        _messageText.text = message;
        _confirmAction = confirmAction;
        _panel.SetActive(true);
    }

    private void OnConfirm()
    {
        _confirmAction?.Invoke();
        _panel.SetActive(false);
    }

    private void OnCancel()
    {
        _panel.SetActive(false);
    }
}