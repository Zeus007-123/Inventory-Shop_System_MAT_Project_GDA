using TMPro;
using UnityEngine;
using System.Collections;

public class MessageController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _messagePanel;
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private float _displayDuration = 2f;

    private Coroutine _currentMessageRoutine;

    void Start()
    {
        Debug.Log("[MessageController] Initializing");
        _messagePanel.SetActive(false);

        var eventService = ServiceLocator.Get<EventService>();

        eventService.OnTransactionMessage.AddListener(ShowMessage);
        eventService.OnTransactionFailed.AddListener(ShowError);

        Debug.Log("[MessageController] Subscribed to OnTransactionMessage and OnTransactionFailed");

    }

    public void ShowMessage(string message)
    {
        Debug.Log($"[Message] Showing: {message}");
        if (_currentMessageRoutine != null)
        {
            StopCoroutine(_currentMessageRoutine);
            _messagePanel.SetActive(false); // Reset before showing new message
        }

        _currentMessageRoutine = StartCoroutine(DisplayMessage(message));
    }

    private IEnumerator DisplayMessage(string message)
    {
        
        Debug.Log("[Message] DisplayMessage coroutine started");
        _messageText.text = message;

        _messagePanel.SetActive(true);
        Debug.Log($"[Message] Panel active: {_messagePanel.activeSelf}");
        Debug.Log($"[Message] Text content: {_messageText.text}");

        yield return new WaitForSeconds(_displayDuration);

        _messagePanel.SetActive(false);
        Debug.Log("[Message] Panel hidden");
        _currentMessageRoutine = null;
    }

    public void ShowError(string errorMessage)
    {
        
        ShowMessage($"<color=#ff0000>{errorMessage}</color>"); // Red text for errors
        Debug.Log($"[Message] Showing error: {errorMessage}");
    }

}