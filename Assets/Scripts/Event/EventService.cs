/*public class EventService
{
    private static EventService instance;
    public static EventService Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventService();
            }
            return instance;
        }
    }

    // Transaction Events
    public EventController<ItemSO, int> OnBuyTransaction { get; private set; }
    public EventController<ItemSO, int> OnSellTransaction { get; private set; }
    public EventController<string> OnTransactionFailed { get; private set; }

    // UI Events
    public EventController<ItemSO, bool> OnItemSelected { get; private set; } // bool: isFromShop

    // System Events
    public EventController OnWeightLimitExceeded { get; private set; }
    public EventController<string> OnTransactionMessage { get; private set; }


    private EventService()
    {
        OnBuyTransaction = new EventController<ItemSO, int>();
        OnSellTransaction = new EventController<ItemSO, int>();
        OnTransactionFailed = new EventController<string>();
        OnItemSelected = new EventController<ItemSO, bool>();
        OnWeightLimitExceeded = new EventController();
        OnTransactionMessage = new EventController<string>();

    }
}
*/
