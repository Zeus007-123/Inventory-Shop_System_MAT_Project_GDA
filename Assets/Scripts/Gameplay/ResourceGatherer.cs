using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Handles resource gathering mechanics in the game. This script allows the player to collect resources,
/// and update inventory accordingly. It manages gathering logic, cooldowns, and triggers relevant events 
/// for resource collection.
/// </summary>

public class ResourceGatherer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button _gatherButton;

    private IInventoryService inventory;
    private EventService eventService;

    private void Start()
    {
        inventory = ServiceLocator.Get<IInventoryService>();
        eventService = ServiceLocator.Get<EventService>();
        _gatherButton.onClick.AddListener(OnGatherResources);
    }

    public void OnGatherResources()
    {

        // Check inventory capacity
        if (inventory.CurrentWeight >= inventory.MaxWeight)
        {
            eventService.OnTransactionFailed.Invoke("Max weight reached!");
            
            return;
        }

        // Dynamic rarity calculation based on inventory value
        ItemRarity rarity = CalculateDynamicRarity(inventory.TotalValue);

        ItemSO item = GetRandomItemByRarity(rarity);
        if (item != null)
        {
            inventory.AddItem(item, 1);
        }
        
    }

    private ItemRarity CalculateDynamicRarity(float inventoryValue)
    {
        // Adjust drop chances based on total inventory value
        List<WeightedValue<ItemRarity>> weights = new()
        {
            new(ItemRarity.VeryCommon, Mathf.Max(40 - (0.1f * inventoryValue), 0)),
            new(ItemRarity.Common, Mathf.Max(30 - (0.05f * inventoryValue), 0)),
            new(ItemRarity.Rare, 20 + (0.1f * inventoryValue)),
            new(ItemRarity.Epic, 8 + (0.05f * inventoryValue)),
            new(ItemRarity.Legendary, 2 + (0.02f * inventoryValue))
        };

        return WeightedRandomizer.GetRandomItem(weights);
    }

    private ItemSO GetRandomItemByRarity(ItemRarity rarity)
    {
        // Fetch a random item of the specified rarity from ShopService
        var shopService = ServiceLocator.Get<IShopService>();

        return shopService.AllItems
        .Where(item => item.ItemRarity == rarity)
        .OrderBy(_ => Random.value)
        .FirstOrDefault();
        
    }
}