/*using System.Collections.Generic;
using System.Linq;

public class ShopService : IShopService
{
    private List<ItemSO> _allItems = new();
    private List<ItemSO> _currentItems = new();

    public void Initialize(List<ItemSO> allItems)
    {
        _allItems = allItems;
        _currentItems = _allItems;
    }

    public List<ItemSO> GetFilteredItems(ItemType type)
    {
        _currentItems = _allItems
            .Where(item => item.ItemType == type || type == ItemType.All)
            .ToList();

        return _currentItems;

        ServiceLocator.Get<ShopPanelController>().InitializeShopSlots(filteredItems);
    }

    public List<ItemSO> AvailableItems => _currentItems;
}*/