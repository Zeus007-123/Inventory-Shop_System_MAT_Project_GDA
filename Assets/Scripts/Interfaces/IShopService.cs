using System.Collections.Generic;

public interface IShopService : IService
{
    void Initialize(List<ItemSO> allItems);
    List<ItemSO> GetFilteredItems(ItemType type);
    List<ItemSO> AvailableItems { get; }
    List<ItemSO> AllItems { get; }
}