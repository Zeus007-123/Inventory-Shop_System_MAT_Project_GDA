/// <summary>
/// Handles in-game transactions for buying and selling items.
/// Interacts with InventoryService, CurrencyService, and EventService 
/// to process transactions and trigger relevant game events.
/// </summary>

public class TransactionService : ITransactionService
{
    private readonly IInventoryService _inventory;
    private readonly ICurrencyService _currency;
    private readonly EventService _eventService;

    // Constructor retrieves required services via ServiceLocator. Ensures dependency injection without hard dependencies.
    public TransactionService()
    {
        _inventory = ServiceLocator.Get<IInventoryService>();
        _currency = ServiceLocator.Get<ICurrencyService>();
        _eventService = ServiceLocator.Get<EventService>();
    }

    // Processes a transaction based on its type (Buy or Sell). Calls appropriate handlers based on the transaction type.
    public void ProcessTransaction(TransactionData data)
    {
        
        try
        {

            switch (data.Type)
            {
                case TransactionType.Buy:
                    HandlePurchase(data);
                    _eventService.OnBuyTransaction.Invoke(data.Item, data.Quantity); // Success sound
                    break;

                case TransactionType.Sell:
                    HandleSale(data);
                    _eventService.OnSellTransaction.Invoke(data.Item, data.Quantity); // Success sound
                    break;

                default:
                    throw new System.NotImplementedException();
            }
            
        }
        catch (System.Exception ex)
        {
            _eventService.OnTransactionFailed.Invoke(ex.Message); //Failure Sound
        }
    }

    // Handles a purchase transaction. Checks if the player has enough currency and inventory space before completing the purchase.
    private void HandlePurchase(TransactionData data)
    {

        if (data.Quantity <= 0)
            throw new System.Exception("Invalid quantity!");

        float totalCost = data.Item.BuyingPrice * data.Quantity;

        // Check if the player has enough coins to make the purchase 
        if (!_currency.TryDeductCoins(totalCost))
            throw new System.Exception("Not enough coins!");

        // Check if there is enough inventory space to store the purchased item
        float totalWeight = data.Item.Weight * data.Quantity;
        if (!_inventory.CanAddItem(totalWeight))
            throw new System.Exception("Inventory full!");

        // Add the item to the inventory and deduct currency
        _currency.TryDeductCoins(totalCost);
        _inventory.AddItem(data.Item, data.Quantity);

        // Notify event listeners of the successful transaction
        _eventService.OnBuyTransaction.Invoke(data.Item, data.Quantity);
        _eventService.OnTransactionMessage.Invoke($"Bought {data.Quantity}x {data.Item.ItemName} for {totalCost}G");

    }

    // Handles a sale transaction. Checks if the player has the required item and quantity before proceeding with the sale.
    private void HandleSale(TransactionData data)
    {

        if (data.Quantity <= 0)
            throw new System.Exception("Invalid quantity!");

        float totalValue = data.Item.SellingPrice * data.Quantity;

        // Check if the player has enough of the item to sell
        if (!_inventory.HasItem(data.Item, data.Quantity))
            throw new System.Exception("Not enough items!");

        // Remove the item from the inventory and add coins to the balance
        _currency.AddCoins(totalValue);
        _inventory.RemoveItem(data.Item, data.Quantity);

        // Notify event listeners of the successful sale
        _eventService.OnInventoryUpdated.Invoke();
        _eventService.OnSellTransaction.Invoke(data.Item, data.Quantity);
        _eventService.OnTransactionMessage.Invoke($"Sold {data.Quantity}x {data.Item.ItemName} for {totalValue}G");

    }
}