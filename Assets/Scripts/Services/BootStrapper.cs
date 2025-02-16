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
    [SerializeField] private float _maxInventoryWeight = 100f; // Replaced hard-coded value
    [SerializeField] private float _startingCoins = 0f;

    [Header("UI References")]
    [SerializeField] private MessageController _messageController;

    void Awake()
    {
        Debug.Log("[Bootstrapper] Initializing services...");

        #region Core Services Initialization
        // Event service must be initialized first as other services depend on it
        ServiceLocator.Register(new EventService());
        Debug.Log("[Bootstrapper] EventService registered");

        // Inventory service with configurable max weight
        ServiceLocator.Register<IInventoryService>(new InventoryService(_maxInventoryWeight));
        Debug.Log($"[Bootstrapper] Registered IInventoryService: {ServiceLocator.Get<IInventoryService>() != null}");
        Debug.Log($"[Bootstrapper] InventoryService registered (Max Weight: {_maxInventoryWeight}kg)");

        // Currency service for handling coins
        ServiceLocator.Register<ICurrencyService>(new CurrencyService(_startingCoins));
        Debug.Log("[Bootstrapper] CurrencyService registered");
        #endregion

        #region Shop Service Initialization
        var shopService = new ShopService();

        // Load all ItemSO assets from Resources/Items folder
        ItemSO[] items = Resources.LoadAll<ItemSO>("Items");
        Debug.Log($"[Bootstrapper] Loaded {items.Length} items from Resources");

        if (items.Length == 0)
        {
            Debug.LogError("[Bootstrapper] No items found in Resources/Items folder!");
        }

        shopService.Initialize(new List<ItemSO>(items));
        ServiceLocator.Register<IShopService>(shopService);
        Debug.Log($"[Bootstrapper] Registered IShopService: {ServiceLocator.Get<IShopService>() != null}");
        Debug.Log("[Bootstrapper] ShopService registered with items");
        #endregion

        #region Audio Service Initialization
        if (_soundConfig == null || _audioSource == null)
        {
            Debug.LogError("[Bootstrapper] Sound configuration missing!");
            return;
        }

        var soundService = new SoundService(_soundConfig, _audioSource);
        soundService.Init(ServiceLocator.Get<EventService>());
        ServiceLocator.Register(soundService);
        Debug.Log("[Bootstrapper] SoundService registered");
        #endregion

        #region UI Service Registration
        if (_messageController == null)
        {
            Debug.LogWarning("[Bootstrapper] MessageController reference missing!");
            return;
        }

        ServiceLocator.Register(_messageController);
        Debug.Log("[Bootstrapper] MessageController registered");
        #endregion

        Debug.Log("[Bootstrapper] All services initialized successfully");
    }
}