using System.Collections.Generic;

public interface IShopService : IService
{
    List<ItemSO> GetFilteredItems(ItemType type);
    List<ItemSO> AvailableItems { get; }
    void Initialize(List<ItemSO> allItems);
}