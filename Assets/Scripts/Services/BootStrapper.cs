using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Initializes and registers all core services at game startup.
/// Acts as the composition root for dependency injection.
/// </summary>
public class Bootstrapper : MonoBehaviour
{
    [Header("Service References")]
    [SerializeField] private SoundSO _soundConfig;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _maxInventoryWeight = 100f; 
    [SerializeField] private float _startingCoins = 0f;

    [Header("UI References")]
    [SerializeField] private MessageController _messageController;

    void Awake()
    {

        #region Core Services Initialization
        // Event service must be initialized first as other services depend on it
        ServiceLocator.Register(new EventService());

        // Inventory service with configurable max weight
        ServiceLocator.Register<IInventoryService>(new InventoryService(_maxInventoryWeight));

        // Currency service for handling coins
        ServiceLocator.Register<ICurrencyService>(new CurrencyService(_startingCoins));

        // Transaction service for transaction processing
        ServiceLocator.Register<ITransactionService>(new TransactionService());

        #endregion

        #region Shop Service Initialization
        var shopService = new ShopService();

        // Load all ItemSO assets from Resources/Items folder
        ItemSO[] items = Resources.LoadAll<ItemSO>("Items");

        shopService.Initialize(new List<ItemSO>(items));
        ServiceLocator.Register<IShopService>(shopService);
     
        #endregion

        #region Audio Service Initialization
        if (_soundConfig == null || _audioSource == null)
        {
            return;
        }

        var soundService = new SoundService(_soundConfig, _audioSource);
        soundService.Init(ServiceLocator.Get<EventService>());
        ServiceLocator.Register(soundService);

        #endregion

        #region UI Service Registration
        if (_messageController == null)
        {
            return;
        }

        ServiceLocator.Register(_messageController);

        #endregion

    }
}