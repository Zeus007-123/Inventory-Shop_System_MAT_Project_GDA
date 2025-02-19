/// <summary>
/// Interface for the currency management system.  
/// Provides methods to add, deduct, and track the player's in-game currency.
/// This service is used for handling financial transactions within the shop-inventory system.
/// </summary>

public interface ICurrencyService : IService
{
    void AddCoins(float amount);
    bool TryDeductCoins(float amount);
    float CurrentCoins { get; }
    
}