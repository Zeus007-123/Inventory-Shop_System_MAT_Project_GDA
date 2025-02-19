using TMPro;
using UnityEngine;

/// <summary>
/// UIManager handles updating the UI elements related to currency and inventory weight.
/// It listens to various events such as currency updates, inventory changes, and transactions 
/// to ensure the displayed values remain accurate.
/// </summary>

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _coinsText; // Displays the player's current coins
    [SerializeField] private TextMeshProUGUI _weightText; // Displays the current inventory weight

    // Subscribes to necessary events and initializes the UI when the script starts.
    void Start()
    {
        var eventService = ServiceLocator.Get<EventService>();

        // Listen for updates to the player's currency and inventory
        eventService.OnCurrencyUpdated.AddListener(UpdateUI);
        eventService.OnInventoryUpdated.AddListener(UpdateUI);

        // Listen for buy and sell transactions to refresh UI after purchases or sales
        eventService.OnBuyTransaction.AddListener(UpdateUIWithParams);
        eventService.OnSellTransaction.AddListener(UpdateUIWithParams);

        UpdateUI(); // Perform an initial UI update to reflect current values
    }

    // Updates the UI with the latest currency amount and inventory weight. Retrieves values from the currency and inventory services.
    private void UpdateUI()
    {

        var currency = ServiceLocator.Get<ICurrencyService>();      // Fetch the currency service
        var inventory = ServiceLocator.Get<IInventoryService>();    // Fetch the inventory service

        // Update UI text with the latest coin balance and weight status
        _coinsText.text = currency.CurrentCoins.ToString();
        _weightText.text = $"{inventory.CurrentWeight} / {inventory.MaxWeight}"; // Show current and max weight

    }

    // Updates the UI in response to a buy or sell transaction.
    // The parameters (item and quantity) are not used directly but can be extended for more UI feedback.
    void UpdateUIWithParams(ItemSO item, int quantity)
    {
        UpdateUI(); // Calls the standard UI update method
    }

}
