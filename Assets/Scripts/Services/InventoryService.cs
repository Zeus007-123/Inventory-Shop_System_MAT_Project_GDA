using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InventoryService : IInventoryService
{
    /*public class InventorySlot
    {
        public ItemSO Item { get; set; }
        public int Quantity { get; set; }
    }*/
    private List<InventorySlot> _slots = new List<InventorySlot>();
    public IEnumerable<InventorySlot> Slots => _slots.AsReadOnly();
    public float CurrentWeight { get; private set; }
    public float MaxWeight { get; }
    public float TotalValue => CalculateTotalValue();
    private const float WeightPrecision = 0.01f;

    public InventoryService(float maxWeight)
    {
        MaxWeight = maxWeight;
        CurrentWeight = 0f;
        Debug.Log($"[Inventory] Service initialized. Max capacity: {MaxWeight}kg");
    }

    public void AddItem(ItemSO item, int quantity)
    {
        if (item == null)
        {
            Debug.LogError("[Inventory] Tried to add null item!");
            return;
        }

        float weightToAdd = item.Weight * quantity;
        Debug.Log($"[Inventory] Attempting to add {quantity}x {item.ItemName} " +
                 $"(Unit weight: {item.Weight}kg, Total: {weightToAdd}kg)");

        if (!CanAddItem(weightToAdd))
        {
            Debug.LogError($"Cannot add {quantity}x {item.ItemName}. " +
                          $"Would exceed weight limit ({CurrentWeight + weightToAdd}/{MaxWeight}kg)");
            ServiceLocator.Get<EventService>().OnTransactionFailed.Invoke("Weight limit exceeded!");
            return;
        }

        
        Debug.Log($"[Inventory] Added {quantity}x {item.ItemName}. " +
                 $"New weight: {CurrentWeight:F2}/{MaxWeight}kg");

        InventorySlot existingSlot = _slots.Find(slot => slot.Item == item);
        
        if (existingSlot != null)
        {
            existingSlot.Quantity += quantity;
            Debug.Log($"[Inventory] Stacked {quantity}x {item.ItemName} (Total: {existingSlot.Quantity})");
        }
        else
        {
            _slots.Add(new InventorySlot { Item = item, Quantity = quantity });
            Debug.Log($"[Inventory] Added new item: {item.ItemName} x{quantity}");
        }

        //Debug.Log($"[Inventory] Stored in slot: {item.ItemName} x{quantity}");
        CurrentWeight += weightToAdd;

        // Add to inventory storage logic here
        ServiceLocator.Get<EventService>().OnInventoryUpdated.Invoke();

        Debug.Log($"[Inventory] Added {quantity}x {item.ItemName} (Weight: {CurrentWeight}/{MaxWeight})");
    }

    // Removes an item from the inventory, ensuring there is enough quantity
    public void RemoveItem(ItemSO item, int quantity)
    {
        var slot = _slots.FirstOrDefault(s => s.Item == item);
        if (slot == null || slot.Quantity < quantity)
        {
            Debug.LogWarning($"Cannot remove {quantity}x {item.ItemName}, not enough quantity in inventory!");
            return;
        }

        slot.Quantity -= quantity;

        // Remove slot if quantity reaches zero
        if (slot.Quantity <= 0)
        {
            _slots.Remove(slot);
        }

        CurrentWeight -= item.Weight * quantity;

        ServiceLocator.Get<EventService>().OnInventoryUpdated.Invoke();

        Debug.Log($"Removed {quantity}x {item.ItemName} from inventory. Current Weight: {CurrentWeight}/{MaxWeight}");
    }

    public bool CanAddItem(float weightToAdd)
    {
        return (CurrentWeight + weightToAdd) <= MaxWeight + WeightPrecision;
    }
    // Checks if the inventory contains a specific item with a required quantity
    public bool HasItem(ItemSO item, int quantity)
    {
        bool hasItem = _slots.Any(s => s.Item == item && s.Quantity >= quantity);
        Debug.Log($"Has {quantity}x {item.ItemName}? {hasItem}");
        return hasItem;
    }

    public int GetItemQuantity(ItemSO item)
    {
        var slot = _slots.Find(s => s.Item == item);
        return slot?.Quantity ?? 0;
    }

    private float CalculateTotalValue()
    {
        float total = 0f;
        foreach (var slot in _slots)
        {
            if (slot.Item != null)
            {
                // Use SellingPrice or another value field from ItemSO
                total += slot.Item.SellingPrice * slot.Quantity;
            }
        }
        Debug.Log($"[Inventory] Total value: {total}G");
        return total;
    }
    public void ResetInventory()
    {
        CurrentWeight = 0f;
        Debug.Log("[Inventory] Reset to 0kg");
    }
}