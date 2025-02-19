/// <summary>
/// Represents a transaction involving an item, including details such as item reference, 
/// quantity, and transaction type (buy or sell).
/// </summary>

public struct TransactionData
{
    public ItemSO Item;
    public int Quantity;
    public TransactionType Type;
}
