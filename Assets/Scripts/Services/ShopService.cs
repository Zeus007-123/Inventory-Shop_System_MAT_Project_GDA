using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopService : IShopService
{
    public List<ItemSO> AllItems { get; private set; }
    public List<ItemSO> AvailableItems { get; private set; }

    // Initializes the shop service with available items
    public void Initialize(List<ItemSO> allItems)
    {
        AllItems = allItems;
        AvailableItems = allItems.ToList(); // Create a copy
        Debug.Log("ShopService Initialized with items");
    }

    // Filters items based on type
    public List<ItemSO> GetFilteredItems(ItemType type)
    {
        AvailableItems = AllItems
            .Where(item => item.ItemType == type || type == ItemType.All)
            .ToList();

        Debug.Log($"Filtered Items by type: {type}");
        return AvailableItems;
    }

    // Explicit interface implementation
    List<ItemSO> IShopService.AvailableItems => AvailableItems;
}