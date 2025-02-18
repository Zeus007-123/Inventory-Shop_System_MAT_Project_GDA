using System.Collections.Generic;

public interface IInventoryService
{

    IEnumerable<InventorySlot> Slots { get; }
    void AddItem(ItemSO item, int quantity);
    void RemoveItem(ItemSO item, int quantity);
    bool CanAddItem(float weight);
    bool HasItem(ItemSO item, int quantity);
    int GetItemQuantity(ItemSO currentItem);

    float CurrentWeight { get; }
    float MaxWeight { get; }
    float TotalValue { get; }
}