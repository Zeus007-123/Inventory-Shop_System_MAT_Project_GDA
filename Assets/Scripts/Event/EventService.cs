using UnityEngine;

/// <summary>
/// EventService manages and broadcasts various game events related to transactions, UI interactions, and system status.
/// </summary>
public class EventService
{
    /*
    // Singleton pattern (commented out, but can be used if needed)
    private static EventService instance;
    public static EventService Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventService();
            }
            return instance;
        }
    }
    */

    // Transaction-related events
    public EventController<float> OnCurrencyUpdated { get; private set; } // Triggered when currency is updated
    public EventController<ItemSO, int> OnBuyTransaction { get; private set; } // Triggered when an item is bought
    public EventController<ItemSO, int> OnSellTransaction { get; private set; } // Triggered when an item is sold
    public EventController<string> OnTransactionFailed { get; private set; } // Triggered when a transaction fails

    // UI-related events
    public EventController OnInventoryUpdated { get; private set; }
    public EventController<ItemSO, bool> OnItemSelected { get; private set; } // Triggered when an item is selected (bool: isFromShop)
    public EventController<ItemSO, bool> OnTransactionInitiated { get; private set; } // Triggered when a transaction starts

    // System-related events
    public EventController OnWeightLimitExceeded { get; private set; } // Triggered when weight limit is reached
    public EventController<string> OnTransactionMessage { get; private set; } // Used for transaction-related messages

    /// <summary>
    /// Constructor initializes all event controllers.
    /// </summary>
    public EventService()
    {
        Debug.Log("[EventService] Initializing event controllers...");

        OnCurrencyUpdated = new EventController<float>();
        OnBuyTransaction = new EventController<ItemSO, int>();
        OnSellTransaction = new EventController<ItemSO, int>();
        OnTransactionFailed = new EventController<string>();
        OnInventoryUpdated = new EventController();
        OnItemSelected = new EventController<ItemSO, bool>();
        OnTransactionInitiated = new EventController<ItemSO, bool>();
        OnWeightLimitExceeded = new EventController();
        OnTransactionMessage = new EventController<string>();

        Debug.Log("[EventService] All event controllers initialized successfully.");
    }
}
