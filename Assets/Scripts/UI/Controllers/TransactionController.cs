using UnityEngine;
using TMPro;

public class TransactionController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _transactionPanel; // UI panel for transaction
    //[SerializeField] private TMP_Text _itemNameText; // Displays the item name
    [SerializeField] private TMP_Text _quantityText; // Displays the selected quantity
    [SerializeField] private TMP_Text _totalPriceText; // Displays the total price (Gold Required)
    [SerializeField] private TMP_Text _maxQuantityText; // Displays max quantity that can be purchased
    [SerializeField] private TMP_Text _cumulativeWeightText; // Displays cumulative weight

    private ItemSO _currentItem; // Stores the selected item
    private int _currentQuantity = 0; // Tracks the selected quantity
    private int _maxPurchasableQuantity = 0; // Maximum quantity that can be bought
    private TransactionType _currentTransactionType; // Stores whether the transaction is a Buy or Sell
    private float _playerGold = 1000f; // Example player gold (Replace with actual player gold reference)
    private float _playerWeightCapacity = 50f; // Example max carry weight (Replace with actual inventory system reference)

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

        CalculateMaxPurchasableQuantity();
        ResetTransactionUI();
        _transactionPanel.SetActive(true);
        Debug.Log("TransactionController: Transaction panel activated.");
    }

    /// <summary>
    /// Calculates the maximum quantity that can be purchased based on player gold and weight capacity.
    /// </summary>
    private void CalculateMaxPurchasableQuantity()
    {
        float pricePerUnit = _currentTransactionType == TransactionType.Buy ? _currentItem.BuyingPrice : _currentItem.SellingPrice;
        _maxPurchasableQuantity = Mathf.Min(
            Mathf.FloorToInt(_playerGold / pricePerUnit), // Based on gold
            Mathf.FloorToInt(_playerWeightCapacity / _currentItem.Weight), // Based on weight capacity
            _currentItem.MaxStackSize // Based on max stack size
        );

        Debug.Log($"TransactionController: Max purchasable quantity calculated as {_maxPurchasableQuantity}");
    }

    /// <summary>
    /// Resets the transaction UI to default values.
    /// </summary>
    private void ResetTransactionUI()
    {
        _currentQuantity = 0;
        UpdateUI();
        Debug.Log("TransactionController: Transaction UI reset.");
    }

    /// <summary>
    /// Increases the quantity of the selected item in the transaction.
    /// </summary>
    public void IncreaseQuantity()
    {
        int previousQuantity = _currentQuantity;
        _currentQuantity = Mathf.Clamp(_currentQuantity + 1, 0, _maxPurchasableQuantity);

        Debug.Log($"TransactionController: Quantity increased from {previousQuantity} to {_currentQuantity}.");
        UpdateUI();
    }

    /// <summary>
    /// Decreases the quantity of the selected item in the transaction.
    /// </summary>
    public void DecreaseQuantity()
    {
        int previousQuantity = _currentQuantity;
        _currentQuantity = Mathf.Clamp(_currentQuantity - 1, 0, _maxPurchasableQuantity);

        Debug.Log($"TransactionController: Quantity decreased from {previousQuantity} to {_currentQuantity}.");
        UpdateUI();
    }

    /// <summary>
    /// Updates the transaction UI with the latest item details.
    /// </summary>
    private void UpdateUI()
    {
        //_itemNameText.text = _currentItem.ItemName;
        _quantityText.text = _currentQuantity.ToString();
        _maxQuantityText.text = $"Max Quantity That Can Be Purchased: {_maxPurchasableQuantity}";
        _cumulativeWeightText.text = $"Cumulative Weight: {_currentQuantity * _currentItem.Weight}kg";

        float pricePerUnit = _currentTransactionType == TransactionType.Buy ? _currentItem.BuyingPrice : _currentItem.SellingPrice;
        _totalPriceText.text = $"Gold Required: {pricePerUnit * _currentQuantity}G";

        Debug.Log($"TransactionController: UI updated - Item: {_currentItem.ItemName}, " +
                  $"Quantity: {_currentQuantity}, Total Price: {pricePerUnit * _currentQuantity}G, " +
                  $"Max Quantity: {_maxPurchasableQuantity}, Cumulative Weight: {_currentQuantity * _currentItem.Weight}");
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
   
    }

    // Hide the transaction panel after confirmation
    public void CancelTransaction()
    {
        _transactionPanel.SetActive(false);
        Debug.Log("TransactionController: Transaction panel deactivated after confirmation.");
    }
}
