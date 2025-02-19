using TMPro;
using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the display of transaction messages and errors in the shop system.
/// This script manages the UI panel that briefly shows success/error messages.
/// </summary>

public class MessageController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _messagePanel;
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private float _displayDuration = 2f; // Duration before message disappears

    private Coroutine _currentMessageRoutine; // Tracks the active message coroutine

    // Initializes the message panel and subscribes to transaction-related events.
    void Start()
    {
        _messagePanel.SetActive(false); // Ensure the message panel is hidden at start

        var eventService = ServiceLocator.Get<EventService>();

        // Listen for transaction success messages
        eventService.OnTransactionMessage.AddListener(ShowMessage);
        // Listen for transaction failure messages (errors)
        eventService.OnTransactionFailed.AddListener(ShowError);

    }

    // Displays a success or informational message in the UI. If another message is currently displaying, it resets and shows the new one.
    public void ShowMessage(string message)
    {
        // If a message is currently being displayed, stop the existing coroutine and reset the panel
        if (_currentMessageRoutine != null)
        {
            StopCoroutine(_currentMessageRoutine);
            _messagePanel.SetActive(false); // Reset before showing new message
        }
        // Start a new coroutine to show the message
        _currentMessageRoutine = StartCoroutine(DisplayMessage(message));
    }

    // Handles the message display logic, ensuring it appears for a set duration.
    private IEnumerator DisplayMessage(string message)
    {
        
        _messageText.text = message; // Set the message text
        _messagePanel.SetActive(true); // Show the message panel

        yield return new WaitForSeconds(_displayDuration); // Wait for the display duration

        _messagePanel.SetActive(false); // Hide the panel after the duration ends
        _currentMessageRoutine = null; // Reset coroutine reference
    }

    // Displays an error message in red to indicate a failed transaction.
    public void ShowError(string errorMessage)
    {
        
        ShowMessage($"<color=#ff0000>{errorMessage}</color>"); // Display error message in red color
    }

}