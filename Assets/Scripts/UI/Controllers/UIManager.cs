using TMPro;
using UnityEngine;

/// <summary>
/// UIManager handles updating the UI elements such as coins and inventory weight.
/// It listens to buy and sell transactions to refresh the displayed values.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _coinsText; // Displays the player's current coins
    [SerializeField] private TextMeshProUGUI _weightText; // Displays the current inventory weight

    void Start()
    {
        var eventService = ServiceLocator.Get<EventService>();

        eventService.OnInventoryUpdated.AddListener(UpdateUI);

        // Subscribe to events for buy and sell transactions
        eventService.OnBuyTransaction.AddListener(UpdateUIWithParams);
        eventService.OnSellTransaction.AddListener(UpdateUIWithParams);

        Debug.Log("UIManager: Subscribed to OnBuyTransaction and OnSellTransaction events.");

        // Initialize UI with current values
        //UpdateUI(null, 0);
        Debug.Log("[UIManager] Subscribed to events.");
        UpdateUI(); // Initial refresh
    }

    /// <summary>
    /// Updates the UI with the latest currency and inventory weight values.
    /// </summary>
    /// <param name="item">The item involved in the transaction (not used directly in UI update).</param>
    /// <param name="quantity">The quantity of the item involved in the transaction (not used directly in UI update).</param>
    private void UpdateUI()
    {
        //float currentCoins = ServiceLocator.Get<CurrencyService>().CurrentCoins;
        //float currentWeight = ServiceLocator.Get<IInventoryService>().CurrentWeight;

        var inventory = ServiceLocator.Get<IInventoryService>();
        var currency = ServiceLocator.Get<ICurrencyService>();

        //_coinsText.text = currentCoins.ToString();

        _coinsText.text = currency.CurrentCoins.ToString();
        _weightText.text = $"{inventory.CurrentWeight} / {inventory.MaxWeight}"; // Include MaxWeight

        Debug.Log($"[UIManager] Updated UI - Weight: {inventory.CurrentWeight}/{inventory.MaxWeight}");
        //Debug.Log($"UIManager: UI updated - Coins: {currentCoins}, Weight: {currentWeight}/1000");
    }

    void UpdateUIWithParams(ItemSO item, int quantity)
    {
        UpdateUI(); // Reuse parameterless version
    }

}
