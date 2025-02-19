using UnityEngine;
using TMPro;

/// <summary>
/// Controls the confirmation panel UI for buy/sell transactions.
/// Displays a message asking the player to confirm their action.
/// </summary>

public class ConfirmationPanelController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _panel; // Panel UI element
    [SerializeField] private TextMeshProUGUI _messageText; // Text for displaying the confirmation message

    private TransactionData _pendingTransaction; // Stores transaction data awaiting confirmation

    // Initializes the confirmation panel by ensuring it is hidden at the start.
    private void Start()
    {
        _panel.SetActive(false); // Ensure the panel is hidden on start
    }

    // Displays the confirmation panel with a formatted transaction message.
    public void ShowConfirmation(TransactionData data)
    {
        _pendingTransaction = data; // Store the transaction data for later processing

        // Format and set the confirmation message based on transaction type
        _messageText.text = _pendingTransaction.Type == TransactionType.Buy
            ? $"Buy {data.Quantity}x {data.Item.ItemName} for {data.Quantity * data.Item.BuyingPrice}G?"
            : $"Sell {data.Quantity}x {data.Item.ItemName} for {data.Quantity * data.Item.SellingPrice}G?";

        _panel.SetActive(true); // Show the confirmation panel
    }

    // Confirms the transaction, processes it via the TransactionService, and updates inventory.
    public void OnConfirm()
    {
        ServiceLocator.Get<ITransactionService>().ProcessTransaction(_pendingTransaction); // Process the stored transaction through the TransactionService
        _panel.SetActive(false); // Hide the confirmation panel after completing the transaction
        ServiceLocator.Get<EventService>().OnInventoryUpdated.Invoke(); // Notify the system that inventory has been updated

    }

    // Cancels the transaction and hides the confirmation panel.
    public void OnCancel()
    {
        _panel.SetActive(false); // Simply hide the panel without performing any action
    }
}
