using System;

public interface ICurrencyService : IService
{
    void AddCoins(float amount);
    bool TryDeductCoins(float amount);
    bool HasSufficientFunds(float amount);
    float CurrentCoins { get; }
    
}