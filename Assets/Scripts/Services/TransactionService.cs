using UnityEngine;

public class TransactionService : ITransactionService
{
    private readonly IInventoryService _inventory;
    private readonly ICurrencyService _currency;
    private readonly EventService _eventService;

    public TransactionService()
    {
        _inventory = ServiceLocator.Get<IInventoryService>();
        _currency = ServiceLocator.Get<ICurrencyService>();
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
            _eventService.OnTransactionFailed.Invoke(ex.Message);
        }
    }

    private void HandlePurchase(TransactionData data)
    {
        float totalCost = data.Item.BuyingPrice * data.Quantity;
        if (!_currency.TryDeductCoins(totalCost))
            throw new System.Exception("Not enough coins!");

        float totalWeight = data.Item.Weight * data.Quantity;
        if (!_inventory.CanAddItem(totalWeight))
            throw new System.Exception("Inventory full!");

        _inventory.AddItem(data.Item, data.Quantity);
        _eventService.OnBuyTransaction.Invoke(data.Item, data.Quantity);
        _eventService.OnTransactionMessage.Invoke($"Bought {data.Quantity}x {data.Item.ItemName} for {totalCost}G");
    }

    private void HandleSale(TransactionData data)
    {
        if (!_inventory.HasItem(data.Item, data.Quantity))
            throw new System.Exception("Not enough items!");

        float totalValue = data.Item.SellingPrice * data.Quantity;
        _currency.AddCoins(totalValue);
        _inventory.RemoveItem(data.Item, data.Quantity);
        _eventService.OnSellTransaction.Invoke(data.Item, data.Quantity);
        _eventService.OnTransactionMessage.Invoke($"Sold {data.Quantity}x {data.Item.ItemName} for {totalValue}G");
    }
}