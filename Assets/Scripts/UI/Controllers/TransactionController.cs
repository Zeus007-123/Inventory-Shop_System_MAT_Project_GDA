using UnityEngine;
using TMPro;

/// <summary>
/// Manages the item transaction system for buying and selling items.
/// This script handles item selection, quantity adjustments, UI updates, and transaction confirmation.
/// </summary>

public class TransactionController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _transactionPanel; // UI panel for transaction
    [SerializeField] private ConfirmationPanelController _confirmationPanel;
    [SerializeField] private TMP_Text _quantityText; // Displays the selected quantity
    [SerializeField] private TMP_Text _totalPriceText; // Displays the total price during buy and sell
    [SerializeField] private TMP_Text _maxQuantityText; // Displays max quantity during buy and sell
    [SerializeField] private TMP_Text _cumulativeWeightText; // Displays cumulative weight

    private ItemSO _currentItem; // Stores the selected item
    private int _currentQuantity; // Tracks the selected quantity
    private int _maxPurchasableQuantity; // Maximum quantity that can be bought or sold
    private TransactionType _currentTransactionType; // Stores whether the transaction is a Buy or Sell

    // Subscribes to the transaction event when the script starts.
    private void Start()
    {
        // Listen for item selection events that initiate transactions
        ServiceLocator.Get<EventService>().OnTransactionInitiated.AddListener(HandleItemSelection);
    }

    // Handles item selection and initializes transaction details. Determines if the transaction is a purchase or sale and sets up the UI accordingly.
    private void HandleItemSelection(ItemSO item, bool isFromShop)
    {
        _transactionPanel.SetActive(true); // Show the transaction panel

        _currentItem = item; // Store the selected item
        _currentTransactionType = isFromShop ? TransactionType.Buy : TransactionType.Sell; // Determine transaction type

        // Fetch player-related data (currency and inventory services)
        var currency = ServiceLocator.Get<ICurrencyService>();
        var inventory = ServiceLocator.Get<IInventoryService>();

        // Calculate available weight and gold based on transaction type
        float availableWeight = isFromShop
            ? inventory.MaxWeight - inventory.CurrentWeight
            : inventory.GetItemQuantity(item) * item.Weight;

        float availableGold = isFromShop ? currency.CurrentCoins : float.MaxValue;

        // Calculate the maximum quantity the player can buy or sell
        CalculateMaxPurchasableQuantity(availableGold,
        availableWeight,
            isFromShop);

        // Reset transaction UI
        ResetTransactionUI();
        
    }

    // Calculates the maximum quantity a player can buy or sell based on gold and weight limits.
    //Ensures the quantity does not exceed the stack size limit.
    private void CalculateMaxPurchasableQuantity(float availableGold, float availableWeight, bool isBuying)
    {
        if (isBuying) // Buying logic
        {
            float pricePerUnit = _currentItem.BuyingPrice;
            _maxPurchasableQuantity = Mathf.Min(
                Mathf.FloorToInt(availableGold / pricePerUnit),                         // Gold constraint
                Mathf.FloorToInt(availableWeight / _currentItem.Weight),                // Weight constraint
                _currentItem.MaxStackSize                                               // Stack limit constraint
            );
        }
        else // Selling logic
        {
            _maxPurchasableQuantity = Mathf.Min(
                ServiceLocator.Get<IInventoryService>().GetItemQuantity(_currentItem),  // Available quantity
                _currentItem.MaxStackSize                                               // Stack limit
            );
        }

        _maxPurchasableQuantity = Mathf.Max(_maxPurchasableQuantity, 0); // Ensure it never goes below zero

    }

    // Resets the transaction UI by setting the quantity to zero and updating the UI.
    private void ResetTransactionUI()
    {
        _currentQuantity = 0; // Start with zero quantity
        UpdateUI();
    }

    // Increases the selected quantity, ensuring it does not exceed the maximum allowed amount.
    public void IncreaseQuantity()
    {
        if (_maxPurchasableQuantity == 0) return;                                           // Prevent increasing if no items can be bought/sold
        
        _currentQuantity = Mathf.Clamp(_currentQuantity + 1, 0, _maxPurchasableQuantity);

        UpdateUI();
    }

    // Decreases the selected quantity, ensuring it does not drop below zero.
    public void DecreaseQuantity()
    {
        if (_currentQuantity == 0) return;                                                  // Prevent decreasing below zero

        _currentQuantity = Mathf.Clamp(_currentQuantity - 1, 0, _maxPurchasableQuantity);

        UpdateUI();
    }

    // Updates the transaction UI with the latest quantity, total price, and weight calculations.
    private void UpdateUI()
    {
        _quantityText.text = _currentQuantity.ToString();
        _maxQuantityText.text = $"Max Quantity: {_maxPurchasableQuantity}";
        
        float pricePerUnit = _currentTransactionType == TransactionType.Buy
            ? _currentItem.BuyingPrice
            : _currentItem.SellingPrice;

        _totalPriceText.text = $"Gold: {pricePerUnit * _currentQuantity}";
        _cumulativeWeightText.text = $"Cumulative Weight: {_currentQuantity * _currentItem.Weight}";
    
    }

    // Confirms the transaction and sends the transaction data for final processing.
    // Opens the confirmation panel before executing the transaction.
    public void ConfirmTransaction()
    {
        if (_currentQuantity <= 0) return;              // Prevent confirming an empty transaction

        // Create transaction data
        var transactionData = new TransactionData
        {
            Item = _currentItem,
            Quantity = _currentQuantity,
            Type = _currentTransactionType
        };

        _confirmationPanel.ShowConfirmation(transactionData); // Show confirmation UI
        _transactionPanel.SetActive(false);                  // Hide the transaction panel
    }

    // Cancels the transaction and hides the transaction panel.
    public void CancelTransaction()
    {
        _transactionPanel.SetActive(false);
    }
}
