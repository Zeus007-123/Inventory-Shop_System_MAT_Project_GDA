using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ConfirmationPanelController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _panel; // Panel UI element
    [SerializeField] private TextMeshProUGUI _messageText; // Text for displaying the confirmation message
    [SerializeField] private Button _confirmButton; // Button to confirm the action
    [SerializeField] private Button _cancelButton; // Button to cancel the action

    private System.Action _confirmAction; // Stores the action to be executed upon confirmation

    private void Start()
    {
        _panel.SetActive(false); // Ensure the panel is hidden on start

        _confirmButton.onClick.AddListener(OnConfirm);
        _cancelButton.onClick.AddListener(OnCancel);

        Debug.Log("ConfirmationPanelController: Initialized and panel set to inactive.");
    }

    /// <summary>
    /// Displays the confirmation panel with a specified message and confirmation action.
    /// </summary>
    /// <param name="message">The message to display in the panel.</param>
    /// <param name="confirmAction">The action to execute upon confirmation.</param>
    public void ShowConfirmation(string message, System.Action confirmAction)
    {
        _panel.SetActive(true);

        if (string.IsNullOrEmpty(message))
        {
            Debug.LogWarning("ConfirmationPanelController: ShowConfirmation called with an empty message.");
            return;
        }

        _messageText.text = message;
        _confirmAction = confirmAction;
        

        Debug.Log($"ConfirmationPanelController: Showing confirmation panel with message - {message}");
    }

    /// <summary>
    /// Executes the confirmation action and hides the panel.
    /// </summary>
    public void OnConfirm()
    {
        Debug.Log("ConfirmationPanelController: Confirm button clicked.");
        _confirmAction?.Invoke();
        _panel.SetActive(false);
        Debug.Log("ConfirmationPanelController: Panel hidden after confirmation.");
    }

    /// <summary>
    /// Hides the panel without executing any action.
    /// </summary>
    public void OnCancel()
    {
        Debug.Log("ConfirmationPanelController: Cancel button clicked. Panel hidden.");
        _panel.SetActive(false);
    }
}
