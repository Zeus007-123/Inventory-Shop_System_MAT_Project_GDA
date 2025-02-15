using System.Collections.Generic;

public interface IShopService
{
    List<ItemSO> AllItems { get; }
    void Initialize(List<ItemSO> items);
    List<ItemSO> GetItemsByCategory(ItemType type);
    List<ItemSO> GetItemsByCategory(string categoryName);
}