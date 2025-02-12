/*using UnityEditor.MPE;
using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [Header("Service References")]
    [SerializeField] private SoundSO _soundConfig;
    [SerializeField] private AudioSource _audioSource;

    [Header("UI References")]
    [SerializeField] private MessageController _messageController;

    void Awake()
    {
        // Core Services
        ServiceLocator.Register(new EventService());
        ServiceLocator.Register(new InventoryService(1000f)); // Max weight 1000
        ServiceLocator.Register(new CurrencyService());

        // Shop Service
        var shopService = new ShopService();
        shopService.Initialize(Resources.LoadAll<ItemSO>("Items")); // Load all items
        ServiceLocator.Register(shopService);

        // Sound Service
        var soundService = new SoundService(_soundConfig, _audioSource);
        soundService.Init();
        ServiceLocator.Register(soundService);

        // UI Service
        ServiceLocator.Register(_messageController);
    }
}*/