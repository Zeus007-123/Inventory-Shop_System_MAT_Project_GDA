using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ResourceGatherer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button _gatherButton;

    private void Start()
    {
        _gatherButton.onClick.AddListener(OnGatherResources);
        Debug.Log("[ResourceGatherer] Initialized with gather button.");
    }

    public void OnGatherResources()
    {
        Debug.Log("[ResourceGatherer] Gather button clicked.");

        var inventory = ServiceLocator.Get<IInventoryService>();
        var eventService = ServiceLocator.Get<EventService>();

        Debug.Log($"[Inventory] Current Weight: {inventory.CurrentWeight}/{inventory.MaxWeight}");

        // Check inventory capacity
        if (inventory.CurrentWeight >= inventory.MaxWeight)
        {
            Debug.LogWarning("[ResourceGatherer] Max weight reached!");
            eventService.OnTransactionFailed.Invoke("Max weight reached!");
            return;
        }

        // Dynamic rarity calculation based on inventory value
        ItemRarity rarity = CalculateDynamicRarity(inventory.TotalValue);
        Debug.Log($"[ResourceGatherer] Calculated rarity: {rarity}");

        ItemSO item = GetRandomItemByRarity(rarity);
        if (item != null)
        {
            inventory.AddItem(item, 1);
            Debug.Log($"[ResourceGatherer] Added item: {item.ItemName} (Rarity: {item.ItemRarity})");
        }
        else
        {
            Debug.LogWarning("[ResourceGatherer] No item found for rarity!");
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

        // Log calculated weights for debugging
        Debug.Log("[ResourceGatherer] Weights:\n" +
                string.Join("\n", weights.Select(w => $"{w.Value}: {w.Weight}")));

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
        /*var items = shopService.AllItems
            .Where(item => item.ItemRarity == rarity)
            .ToList();

        if (items.Count == 0)
        {
            Debug.LogWarning($"[ResourceGatherer] No items found for rarity: {rarity}");
            return null;
        }

        return items.OrderBy(_ => Random.value).First();*/
    }
}