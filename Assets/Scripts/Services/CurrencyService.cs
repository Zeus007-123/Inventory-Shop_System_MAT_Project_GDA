using UnityEngine;

public class CurrencyService : ICurrencyService
{
    public float CurrentCoins { get; private set; }

    public CurrencyService(float startingCoins)
    {
        CurrentCoins = startingCoins;
    }

    public bool TryDeductCoins(float amount)
    {
        if (CurrentCoins >= amount)
        {
            CurrentCoins -= amount;
            return true;
        }
        return false;
    }

    public void AddCoins(float amount) => CurrentCoins += amount;
}