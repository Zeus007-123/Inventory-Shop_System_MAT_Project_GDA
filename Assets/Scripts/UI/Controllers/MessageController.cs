using UnityEngine;
using TMPro;
using System.Collections;

public class MessageController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _messageText; // UI text component for displaying messages
    [SerializeField] private float _displayDuration = 2f; // Duration the message remains visible

    private Coroutine _activeCoroutine; // Reference to the active coroutine

    private void Start()
    {
        // Ensure the message text is initially hidden
        _messageText.gameObject.SetActive(false);

        // Subscribe to event listeners for transaction messages and failures
        ServiceLocator.Get<EventService>().OnTransactionFailed.AddListener(ShowMessage);
        ServiceLocator.Get<EventService>().OnTransactionMessage.AddListener(ShowMessage);

        Debug.Log("MessageController: Initialized and subscribed to events.");
    }

    /// <summary>
    /// Displays a message when an event is triggered.
    /// </summary>
    /// <param name="message">The message to display.</param>
    private void ShowMessage(string message)
    {
        Debug.Log($"MessageController: Received message - {message}");

        // Stop any existing message coroutine to prevent overlapping messages
        if (_activeCoroutine != null)
        {
            StopCoroutine(_activeCoroutine);
            Debug.Log("MessageController: Stopped previous message coroutine.");
        }

        // Start a new coroutine to display the message
        _activeCoroutine = StartCoroutine(DisplayMessage(message));
    }

    /// <summary>
    /// Coroutine to display the message for a set duration.
    /// </summary>
    /// <param name="message">The message to display.</param>
    private IEnumerator DisplayMessage(string message)
    {
        _messageText.text = message;
        _messageText.gameObject.SetActive(true);
        Debug.Log($"MessageController: Displaying message - \"{message}\" for {_displayDuration} seconds.");

        yield return new WaitForSeconds(_displayDuration);

        _messageText.gameObject.SetActive(false);
        Debug.Log("MessageController: Message hidden.");
    }
}
