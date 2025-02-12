public interface ITransactionService : IService
{
    void ProcessTransaction(TransactionData data);
}