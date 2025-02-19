/// <summary>
/// Interface for handling transactions in the shop-inventory system.
/// This service is responsible for processing buying and selling transactions.
/// </summary>

public interface ITransactionService : IService
{
    // Processes a transaction based on the provided transaction data.
    // Handles both buying and selling operations by updating inventory and currency accordingly.
    void ProcessTransaction(TransactionData data);
}