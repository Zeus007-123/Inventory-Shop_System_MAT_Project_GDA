/// <summary>
/// Manages the player's currency, allowing for coin additions and deductions.
/// Notifies the system of any currency changes using the EventService.
/// </summary>
public class CurrencyService : ICurrencyService
{
    public float CurrentCoins { get; private set; } // The current amount of coins the player has.

    // Initializes the currency service with a starting amount of coins.
    public CurrencyService(float startingCoins)
    {
        CurrentCoins = startingCoins;
    }

    // Adds the specified amount of coins to the player's balance and triggers the currency update event.
    public void AddCoins(float amount)
    {
        CurrentCoins += amount;

        // Notify other systems that the currency value has changed.
        ServiceLocator.Get<EventService>().OnCurrencyUpdated.Invoke();
    }

    // Attempts to deduct the specified amount of coins from the player's balance.
    // If the player has enough coins, the amount is deducted, and the currency update event is triggered.
    public bool TryDeductCoins(float amount)
    {
        if (CurrentCoins >= amount)
        {
            CurrentCoins -= amount;

            // Notify other systems that the currency value has changed.
            ServiceLocator.Get<EventService>().OnCurrencyUpdated.Invoke();
            return true;
        }

        return false;
    }
}
