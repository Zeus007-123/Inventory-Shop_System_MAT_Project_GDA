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
    private int _currentQuantity; // Tracks the selected quantity
    private int _maxPurchasableQuantity; // Maximum quantity that can be bought
    private TransactionType _currentTransactionType; // Stores whether the transaction is a Buy or Sell
    
    private void Start()
    {
        // Subscribe to the OnItemSelected event
        ServiceLocator.Get<EventService>().OnTransactionInitiated.AddListener(HandleItemSelection);
        Debug.Log("TransactionController: Initialized and subscribed to OnItemSelected event.");
    }

    /// <summary>
    /// Handles item selection for a transaction.
    /// </summary>
    /// <param name="item">The selected item.</param>
    /// <param name="isFromShop">True if the item is being bought; false if selling.</param>
    private void HandleItemSelection(ItemSO item, bool isFromShop)
    {
        _transactionPanel.SetActive(true);

        _currentItem = item;
        _currentTransactionType = isFromShop ? TransactionType.Buy : TransactionType.Sell;

        Debug.Log($"TransactionController: Item selected - {_currentItem.ItemName}, " +
                  $"Transaction Type: {_currentTransactionType}");

        // Get actual player data
        var currency = ServiceLocator.Get<ICurrencyService>();
        var inventory = ServiceLocator.Get<IInventoryService>();

        float availableWeight = _currentTransactionType == TransactionType.Buy
            ? inventory.MaxWeight - inventory.CurrentWeight
            : float.MaxValue;

        CalculateMaxPurchasableQuantity(currency.CurrentCoins,
        availableWeight,
            isFromShop);

        ResetTransactionUI();
        
        Debug.Log("TransactionController: Transaction panel activated.");
    }

    /// <summary>
    /// Calculates the maximum quantity that can be purchased based on player gold and weight capacity.
    /// </summary>
    private void CalculateMaxPurchasableQuantity(float availableGold, float availableWeight, bool isBuying)
    {
        if (isBuying)
        {
            _maxPurchasableQuantity = Mathf.Min(
                Mathf.FloorToInt(availableGold / _currentItem.BuyingPrice),
                Mathf.FloorToInt(availableWeight / _currentItem.Weight),
                _currentItem.MaxStackSize
            );
        }
        else // Selling
        {
            _maxPurchasableQuantity = Mathf.Min(
                ServiceLocator.Get<IInventoryService>().GetItemQuantity(_currentItem),
                _currentItem.MaxStackSize
            );
        }

        _maxPurchasableQuantity = Mathf.Max(_maxPurchasableQuantity, 0);

        if (_maxPurchasableQuantity < 0)
            _maxPurchasableQuantity = 0;

        Debug.Log($"Max Quantity: {_maxPurchasableQuantity}");

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
        if (_maxPurchasableQuantity == 0) return;
        _currentQuantity = Mathf.Clamp(_currentQuantity + 1, 0, _maxPurchasableQuantity);

        Debug.Log($"TransactionController: Quantity increased from {_maxPurchasableQuantity} to {_currentQuantity}.");
        UpdateUI();
    }

    /// <summary>
    /// Decreases the quantity of the selected item in the transaction.
    /// </summary>
    public void DecreaseQuantity()
    {
        if (_currentQuantity == 0) return;
        _currentQuantity = Mathf.Clamp(_currentQuantity - 1, 0, _maxPurchasableQuantity);

        Debug.Log($"TransactionController: Quantity decreased from {_currentQuantity} to {_currentQuantity}.");
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
        
        float pricePerUnit = _currentTransactionType == TransactionType.Buy
            ? _currentItem.BuyingPrice
            : _currentItem.SellingPrice;

        _totalPriceText.text = $"Gold Required: {pricePerUnit * _currentQuantity}";
        _cumulativeWeightText.text = $"Cumulative Weight: {_currentQuantity * _currentItem.Weight}";

        Debug.Log($"TransactionController: UI updated - Item: {_currentItem.ItemName}, " +
                  $"Quantity: {_currentQuantity}, Total Price: {pricePerUnit * _currentQuantity}G, " +
                  $"Max Quantity: {_maxPurchasableQuantity}, Cumulative Weight: {_currentQuantity * _currentItem.Weight}");
    }

    /// <summary>
    /// Confirms the transaction and sends the transaction data for processing.
    /// </summary>
    public void ConfirmTransaction()
    {
        if (_currentQuantity <= 0)
        {
            Debug.LogWarning("Transaction cancelled: Quantity is zero");
            return;
        }

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

        _transactionPanel.SetActive(false);
    }

    // Hide the transaction panel after confirmation
    public void CancelTransaction()
    {
        _transactionPanel.SetActive(false);
        Debug.Log("TransactionController: Transaction panel deactivated after confirmation.");
    }
}
