/*using UnityEditor.MPE;
using UnityEngine;

public class TransactionService : ITransactionService
{
    private readonly InventoryService _inventory;
    private readonly CurrencyService _currency;
    private readonly EventService _eventService;

    public TransactionService()
    {
        _inventory = ServiceLocator.Get<InventoryService>();
        _currency = ServiceLocator.Get<CurrencyService>();
        _eventService = ServiceLocator.Get<EventService>();
    }

    public void ProcessTransaction(TransactionData data)
    {
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
            _eventService.OnTransactionFailed.Trigger(ex.Message);
        }
    }

    private void HandlePurchase(TransactionData data)
    {
        // Validate funds
        float totalCost = data.Item.BuyingPrice * data.Quantity;
        if (!_currency.TryDeductCoins(totalCost))
            throw new System.Exception("Not enough coins!");

        // Validate weight
        float totalWeight = data.Item.Weight * data.Quantity;
        if (!_inventory.CanAddItem(totalWeight))
            throw new System.Exception("Inventory full!");

        // Execute transaction
        _inventory.AddItem(data.Item, data.Quantity);
        _eventService.OnBuyTransaction.Trigger(data.Item, data.Quantity);

        ShowTransactionMessage($"Bought {data.Quantity}x {data.Item.ItemName} for {totalCost}G");
    }

    private void HandleSale(TransactionData data)
    {
        // Validate item availability
        if (!_inventory.HasItem(data.Item, data.Quantity))
            throw new System.Exception("Not enough items!");

        // Execute transaction
        float totalValue = data.Item.SellingPrice * data.Quantity;
        _currency.AddCoins(totalValue);
        _inventory.RemoveItem(data.Item, data.Quantity);
        _eventService.OnSellTransaction.Trigger(data.Item, data.Quantity);

        ShowTransactionMessage($"Sold {data.Quantity}x {data.Item.ItemName} for {totalValue}G");
    }

    private void ShowTransactionMessage(string message)
    {
        _eventService.OnTransactionMessage.Trigger(message);
        _eventService.OnPlaySound.Trigger(SoundTypes.TransactionClick);
    }
}*/