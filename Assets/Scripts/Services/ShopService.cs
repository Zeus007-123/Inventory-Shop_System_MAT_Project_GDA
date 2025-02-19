using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Handles the shop's inventory, allowing filtering and retrieval of items based on category.
/// Implements the IShopService interface for dependency injection and modular design.
/// </summary>

public class ShopService : IShopService
{
    private List<ItemSO> _allItems = new(); // Stores all items available in the shop.
    private const string _defaultCategory = "All"; // Default category name used when retrieving all items.

    public List<ItemSO> AllItems => _allItems; // Gets the list of all available shop items.

    // Initializes the shop with a predefined list of items.
    public void Initialize(List<ItemSO> items)
    {
        _allItems = items;
    }

    // Retrieves items based on a specific item type.
    public List<ItemSO> GetItemsByCategory(ItemType type)
    {
        if (type == ItemType.All)
            return _allItems;

        return _allItems.Where(item => item.ItemType == type).ToList();
    }

    // Retrieves items based on a category name string.
    public List<ItemSO> GetItemsByCategory(string categoryName)
    {
        if (categoryName == _defaultCategory)
            return _allItems;

        return _allItems.Where(item =>
            item.ItemType.ToString().Equals(categoryName, System.StringComparison.OrdinalIgnoreCase)
        ).ToList();
    }
}