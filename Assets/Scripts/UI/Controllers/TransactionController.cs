using UnityEngine;
using TMPro;

public class TransactionController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _transactionPanel; // UI panel for transaction
    [SerializeField] private TMP_Text _itemNameText; // Displays the item name
    [SerializeField] private TMP_Text _quantityText; // Displays the selected quantity
    [SerializeField] private TMP_Text _totalPriceText; // Displays the total price

    private ItemSO _currentItem; // Stores the selected item
    private int _currentQuantity = 1; // Tracks the selected quantity
    private TransactionType _currentTransactionType; // Stores whether the transaction is a Buy or Sell

    private void Start()
    {
        // Subscribe to the OnItemSelected event
        ServiceLocator.Get<EventService>().OnItemSelected.AddListener(HandleItemSelection);
        Debug.Log("TransactionController: Initialized and subscribed to OnItemSelected event.");
    }

    /// <summary>
    /// Handles item selection for a transaction.
    /// </summary>
    /// <param name="item">The selected item.</param>
    /// <param name="isFromShop">True if the item is being bought; false if selling.</param>
    private void HandleItemSelection(ItemSO item, bool isFromShop)
    {
        _currentItem = item;
        _currentTransactionType = isFromShop ? TransactionType.Buy : TransactionType.Sell;

        Debug.Log($"TransactionController: Item selected - {_currentItem.ItemName}, " +
                  $"Transaction Type: {_currentTransactionType}");

        ResetTransactionUI();
        _transactionPanel.SetActive(true);
        Debug.Log("TransactionController: Transaction panel activated.");
    }

    /// <summary>
    /// Resets the transaction UI to default values.
    /// </summary>
    private void ResetTransactionUI()
    {
        _currentQuantity = 1;
        UpdateUI();
        Debug.Log("TransactionController: Transaction UI reset.");
    }

    /// <summary>
    /// Increases the quantity of the selected item in the transaction.
    /// </summary>
    public void IncreaseQuantity()
    {
        int previousQuantity = _currentQuantity;
        _currentQuantity = Mathf.Clamp(_currentQuantity + 1, 1, _currentItem.MaxStackSize);

        Debug.Log($"TransactionController: Quantity increased from {previousQuantity} to {_currentQuantity}.");
        UpdateUI();
    }

    /// <summary>
    /// Decreases the quantity of the selected item in the transaction.
    /// </summary>
    public void DecreaseQuantity()
    {
        int previousQuantity = _currentQuantity;
        _currentQuantity = Mathf.Clamp(_currentQuantity - 1, 1, _currentItem.MaxStackSize);

        Debug.Log($"TransactionController: Quantity decreased from {previousQuantity} to {_currentQuantity}.");
        UpdateUI();
    }

    /// <summary>
    /// Updates the transaction UI with the latest item details.
    /// </summary>
    private void UpdateUI()
    {
        _itemNameText.text = _currentItem.ItemName;
        _quantityText.text = _currentQuantity.ToString();

        float pricePerUnit = _currentTransactionType == TransactionType.Buy ?
            _currentItem.BuyingPrice : _currentItem.SellingPrice;

        _totalPriceText.text = $"Total: {pricePerUnit * _currentQuantity}G";

        Debug.Log($"TransactionController: UI updated - Item: {_currentItem.ItemName}, " +
                  $"Quantity: {_currentQuantity}, Total Price: {pricePerUnit * _currentQuantity}G");
    }

    /// <summary>
    /// Confirms the transaction and sends the transaction data for processing.
    /// </summary>
    public void ConfirmTransaction()
    {
        Debug.Log($"TransactionController: Confirming transaction - {_currentItem.ItemName}, " +
                  $"Quantity: {_currentQuantity}, Type: {_currentTransactionType}");

        // Create transaction data and send it for processing
        ServiceLocator.Get<TransactionService>().ProcessTransaction(
            new TransactionData
            {
                Item = _currentItem,
                Quantity = _currentQuantity,
                Type = _currentTransactionType
            }
        );

        // Hide the transaction panel after confirmation
        _transactionPanel.SetActive(false);
        Debug.Log("TransactionController: Transaction panel deactivated after confirmation.");
    }
}
