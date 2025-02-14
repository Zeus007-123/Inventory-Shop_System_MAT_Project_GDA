using System.Collections.Generic;

public interface IInventoryService : IService
{
    IEnumerable<InventoryService.InventorySlot> Slots { get; }
    void AddItem(ItemSO item, int quantity);
    void RemoveItem(ItemSO item, int quantity);
    bool CanAddItem(float weight);
    bool HasItem(ItemSO item, int quantity);
    float CurrentWeight { get; }
    float MaxWeight { get; }
    float TotalValue { get; }
}