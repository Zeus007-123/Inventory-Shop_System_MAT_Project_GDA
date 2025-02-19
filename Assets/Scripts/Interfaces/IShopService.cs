using System.Collections.Generic;

/// <summary>
/// Interface for the shop service, responsible for managing shop items and their categorization.
/// This service provides methods to retrieve and initialize shop inventory.
/// </summary>

public interface IShopService
{
    List<ItemSO> AllItems { get; } // Gets the list of all available items in the shop.
    void Initialize(List<ItemSO> items); // Initializes the shop with a predefined list of items.
    List<ItemSO> GetItemsByCategory(ItemType type); // Retrieves items from the shop based on their item type.
    List<ItemSO> GetItemsByCategory(string categoryName); // Retrieves items from the shop based on a category name.
                                                         // This method allows filtering using a string category, useful for UI-based selection.
}