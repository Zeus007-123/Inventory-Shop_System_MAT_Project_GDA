public class CurrencyService : ICurrencyService
{
    public float CurrentCoins { get; private set; }

    public CurrencyService(float startingCoins)
    {
        CurrentCoins = startingCoins;
    }

    public void AddCoins(float amount)
    {
        CurrentCoins += amount;
        ServiceLocator.Get<EventService>().OnCurrencyUpdated.Invoke();
    }

    public bool TryDeductCoins(float amount)
    {
        if (CurrentCoins >= amount)
        {
            CurrentCoins -= amount;
            ServiceLocator.Get<EventService>().OnCurrencyUpdated.Invoke();
            return true;
        }
        return false;
    }

    
}