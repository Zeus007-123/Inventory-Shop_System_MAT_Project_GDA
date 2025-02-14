using UnityEngine;

public class CurrencyService : ICurrencyService
{
    private float _coins;
    private EventService _eventService;

    public float CurrentCoins => _coins;

    public CurrencyService()
    {
        _eventService = ServiceLocator.Get<EventService>();
    }

    public void AddCoins(float amount)
    {
        _coins += amount;
        _eventService.OnCurrencyUpdated.Invoke(_coins);
        _eventService.OnSellTransaction.Invoke(null, (int)amount); // Trigger sell event for sound
    }

    public bool TryDeductCoins(float amount)
    {
        if (!HasSufficientFunds(amount))
        {
            _eventService.OnTransactionFailed.Invoke("Not enough coins!");
            return false;
        }

        _coins -= amount;
        _eventService.OnCurrencyUpdated.Invoke(_coins);
        _eventService.OnBuyTransaction.Invoke(null, (int)amount); // Trigger buy event for sound
        return true;
    }

    public bool HasSufficientFunds(float amount) => _coins >= amount;
}