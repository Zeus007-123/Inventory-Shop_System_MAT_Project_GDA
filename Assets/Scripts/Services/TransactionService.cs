using UnityEngine;

public class TransactionService : ITransactionService
{
    private readonly IInventoryService _inventory;
    private readonly ICurrencyService _currency;
    private readonly EventService _eventService;

    // Constructor retrieves required services via ServiceLocator
    public TransactionService()
    {
        _inventory = ServiceLocator.Get<IInventoryService>();
        _currency = ServiceLocator.Get<ICurrencyService>();
        _eventService = ServiceLocator.Get<EventService>();
        Debug.Log("TransactionService Initialized");
    }

    // Processes a transaction based on its type (Buy or Sell)
    public void ProcessTransaction(TransactionData data)
    {
        Debug.Log($"Processing transaction: {data.Type} {data.Quantity}x {data.Item.ItemName}");
        
        try
        {
            switch (data.Type)
            {
                case TransactionType.Buy:
                    HandlePurchase(data);
                    break;

                case TransactionType.Sell:
                    HandleSale(data);
                    break;

                default:
                    throw new System.NotImplementedException();
            }
        }
        catch (System.Exception ex)
        {
            _eventService.OnTransactionFailed.Invoke(ex.Message);
            Debug.LogError($"Transaction failed: {ex.Message}");
        }
    }

    // Handles a purchase transaction
    private void HandlePurchase(TransactionData data)
    {
        // In HandlePurchase
        if (data.Quantity <= 0)
            throw new System.Exception("Invalid quantity!");

        float totalCost = data.Item.BuyingPrice * data.Quantity;
        Debug.Log($"Attempting to buy {data.Quantity}x {data.Item.ItemName} for {totalCost}G");

        // Check if the player has enough coins
        if (!_currency.TryDeductCoins(totalCost))
            throw new System.Exception("Not enough coins!");

        // Check if there is enough inventory space
        float totalWeight = data.Item.Weight * data.Quantity;
        if (!_inventory.CanAddItem(totalWeight))
            throw new System.Exception("Inventory full!");

        // Add the item to the inventory
        _inventory.AddItem(data.Item, data.Quantity);
        _eventService.OnBuyTransaction.Invoke(data.Item, data.Quantity);
        _eventService.OnTransactionMessage.Invoke($"Bought {data.Quantity}x {data.Item.ItemName} for {totalCost}G");

        Debug.Log($"Successfully purchased {data.Quantity}x {data.Item.ItemName} for {totalCost}G");
    }

    // Handles a sale transaction
    private void HandleSale(TransactionData data)
    {
        // In HandleSale
        if (data.Quantity <= 0)
            throw new System.Exception("Invalid quantity!");

        float totalValue = data.Item.SellingPrice * data.Quantity;
        Debug.Log($"Attempting to sell {data.Quantity}x {data.Item.ItemName} for {totalValue}G");

        // Check if the player has enough of the item to sell
        if (!_inventory.HasItem(data.Item, data.Quantity))
            throw new System.Exception("Not enough items!");

        // Remove the item from the inventory and add coins to the balance
        _inventory.RemoveItem(data.Item, data.Quantity);
        _currency.AddCoins(totalValue);
        _eventService.OnSellTransaction.Invoke(data.Item, data.Quantity);
        _eventService.OnTransactionMessage.Invoke($"Sold {data.Quantity}x {data.Item.ItemName} for {totalValue}G");

        Debug.Log($"Successfully sold {data.Quantity}x {data.Item.ItemName} for {totalValue}G");
    }
}