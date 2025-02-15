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

        //ServiceLocator.Get<EventService>().OnInventoryUpdated.AddListener(UpdateUI);

        // Subscribe to events for buy and sell transactions
        eventService.OnBuyTransaction.AddListener(UpdateUI);
        eventService.OnSellTransaction.AddListener(UpdateUI);

        Debug.Log("UIManager: Subscribed to OnBuyTransaction and OnSellTransaction events.");

        // Initialize UI with current values
        //UpdateUI(null, 0);
    }

    /// <summary>
    /// Updates the UI with the latest currency and inventory weight values.
    /// </summary>
    /// <param name="item">The item involved in the transaction (not used directly in UI update).</param>
    /// <param name="quantity">The quantity of the item involved in the transaction (not used directly in UI update).</param>
    void UpdateUI(ItemSO item, int quantity)
    {
        float currentCoins = ServiceLocator.Get<CurrencyService>().CurrentCoins;
        float currentWeight = ServiceLocator.Get<IInventoryService>().CurrentWeight;

        _coinsText.text = currentCoins.ToString();

        var inventory = ServiceLocator.Get<IInventoryService>();
        _weightText.text = $"{inventory.CurrentWeight} / {inventory.MaxWeight}"; // Include MaxWeight

        Debug.Log($"UIManager: UI updated - Coins: {currentCoins}, Weight: {currentWeight}/1000");
    }
}
