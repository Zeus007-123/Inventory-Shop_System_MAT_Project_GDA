using UnityEngine;

public class CurrencyService : ICurrencyService
{
    private float _coins; // Stores the player's current balance
    private EventService _eventService; // Reference to the event system

    public float CurrentCoins => _coins; // Public getter for the current coin balance

    // Constructor initializes event service
    public CurrencyService()
    {
        _eventService = ServiceLocator.Get<EventService>();
        Debug.Log("CurrencyService Initialized. Current Balance:" + _coins);
    }

    // Adds coins to the player's balance and triggers an update event
    public void AddCoins(float amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning("Attempted to add a non-positive amount of coins: " + amount);
            return;
        }
        _coins += amount;
        _eventService.OnCurrencyUpdated.Invoke(_coins);
        Debug.Log($"Added {amount} coins. Current Balance: {_coins}");
        _eventService.OnSellTransaction.Invoke(null, (int)amount); // Trigger sell event for sound
    }

    // Attempts to deduct coins from the balance
    public bool TryDeductCoins(float amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning("Attempted to deduct a non-positive amount of coins: " + amount);
            return false;
        }

        if (!HasSufficientFunds(amount))
        {
            _eventService.OnTransactionFailed.Invoke("Not enough coins!");
            Debug.LogError("Transaction failed: Not enough coins");
            return false;
        }

        _coins -= amount;
        _eventService.OnCurrencyUpdated.Invoke(_coins);
        Debug.Log($"Deducted {amount} coins. Current Balance: {_coins}");
        _eventService.OnBuyTransaction.Invoke(null, (int)amount); // Trigger buy event for sound
        return true;
    }

    // Checks if the player has enough coins for a transaction
    public bool HasSufficientFunds(float amount)
    {
        bool hasFunds = _coins >= amount;
        Debug.Log($"Has sufficient funds for {amount}? {hasFunds}");
        return hasFunds;
    }
}