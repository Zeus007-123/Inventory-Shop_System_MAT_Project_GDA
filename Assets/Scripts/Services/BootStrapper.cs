using UnityEngine;
using System.Collections.Generic;

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
        ServiceLocator.Register(new InventoryService(1000f)); // Max weight
        ServiceLocator.Register(new CurrencyService());

        // Shop Service
        var shopService = new ShopService();
        ItemSO[] items = Resources.LoadAll<ItemSO>("Items");
        shopService.Initialize(new List<ItemSO>(items)); // Convert array to list
        ServiceLocator.Register(shopService);

        // Sound Service
        var soundService = new SoundService(_soundConfig, _audioSource);
        soundService.Init(ServiceLocator.Get<EventService>()); // Pass EventService
        ServiceLocator.Register(soundService);

        // UI Service
        ServiceLocator.Register(_messageController);
    }
}