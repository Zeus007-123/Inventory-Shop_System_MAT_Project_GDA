using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages shop inventory, filtering, and item data.
/// Implements IShopService for dependency injection.
/// </summary>
public class ShopService : IShopService
{
    private List<ItemSO> _allItems = new List<ItemSO>();
    private const string _defaultCategory = "All";

    // Explicit interface implementation
    public List<ItemSO> AllItems => _allItems;

    public void Initialize(List<ItemSO> items)
    {
        _allItems = items;
        Debug.Log($"[ShopService] Initialized with {_allItems.Count} items.");
    }

    public List<ItemSO> GetItemsByCategory(ItemType type)
    {
        if (type == ItemType.All)
            return _allItems;

        Debug.Log($"[Shop] Filtering by {type}");
        return _allItems.Where(item => item.ItemType == type).ToList();
    }

    public List<ItemSO> GetItemsByCategory(string categoryName)
    {
        if (categoryName == _defaultCategory)
            return _allItems;

        return _allItems.Where(item =>
            item.ItemType.ToString().Equals(categoryName, System.StringComparison.OrdinalIgnoreCase)
        ).ToList();
    }
}