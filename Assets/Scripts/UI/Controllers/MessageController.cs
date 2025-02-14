using UnityEngine;
using TMPro;
using System.Collections;

public class MessageController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private float _displayDuration = 2f;

    private Coroutine _activeCoroutine;

    private void Start()
    {
        _messageText.gameObject.SetActive(false);
        ServiceLocator.Get<EventService>().OnTransactionFailed.AddListener(ShowMessage);
        ServiceLocator.Get<EventService>().OnTransactionMessage.AddListener(ShowMessage);
    }

    private void ShowMessage(string message)
    {
        if (_activeCoroutine != null)
            StopCoroutine(_activeCoroutine);

        _activeCoroutine = StartCoroutine(DisplayMessage(message));
    }

    private IEnumerator DisplayMessage(string message)
    {
        _messageText.text = message;
        _messageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(_displayDuration);
        _messageText.gameObject.SetActive(false);
    }
}