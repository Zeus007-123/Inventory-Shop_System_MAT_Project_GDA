/*using UnityEditor.MPE;
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
        _eventService.OnPlaySound.Trigger(SoundTypes.TransactionClick);
        _eventService.OnCurrencyUpdated.Trigger(_coins);
    }

    public bool TryDeductCoins(float amount)
    {
        if (_coins < amount)
        {
            _eventService.OnPlaySound.Trigger(SoundTypes.FailedClick);
            _eventService.OnTransactionFailed.Trigger("Not enough coins!");
            return false;
        }

        _coins -= amount;
        _eventService.OnCurrencyUpdated.Trigger(_coins);
        return true;
    }

    public bool HasSufficientFunds(float amount) => _coins >= amount;
}*/