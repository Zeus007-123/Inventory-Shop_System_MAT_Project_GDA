using System.Collections.Generic;
using System.Linq;

public class ShopService : IShopService
{
    public List<ItemSO> AllItems { get; private set; }
    public List<ItemSO> AvailableItems { get; private set; }

    public void Initialize(List<ItemSO> allItems)
    {
        AllItems = allItems;
        AvailableItems = allItems.ToList(); // Create a copy
    }

    public List<ItemSO> GetFilteredItems(ItemType type)
    {
        AvailableItems = AllItems
            .Where(item => item.ItemType == type || type == ItemType.All)
            .ToList();

        return AvailableItems;
    }

    // Explicit interface implementation
    List<ItemSO> IShopService.AvailableItems => AvailableItems;
}