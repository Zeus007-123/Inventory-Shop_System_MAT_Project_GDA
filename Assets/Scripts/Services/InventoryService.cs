using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the inventory system, handling item storage, weight management, and inventory transactions.
/// Ensures items can be added and removed while enforcing weight limits.
/// </summary>

public class InventoryService : IInventoryService
{
    
    private List<InventorySlot> _slots = new List<InventorySlot>(); // The list of inventory slots, each containing an item and its quantity.
    private const float WeightPrecision = 0.01f; // A small precision value to avoid floating-point calculation issues when checking weight constraints.
    
    public IEnumerable<InventorySlot> Slots => _slots.AsReadOnly(); // Provides a read-only collection of inventory slots, ensuring external scripts cannot modify the list directly.
    public float CurrentWeight { get; private set; } // The current total weight of all items in the inventory.
    public float MaxWeight { get; } // The maximum weight capacity the inventory can hold.
    public float TotalValue => CalculateTotalValue(); // The total value of all items in the inventory, calculated dynamically.
    
    // Constructor to initialize max weight and current weight.
    public InventoryService(float maxWeight)
    {
        MaxWeight = maxWeight;
        CurrentWeight = 0f;
    }

    // Adds an item to the inventory, updating the quantity if it already exists. Checks weight limits before adding.
    public void AddItem(ItemSO item, int quantity)
    {
        if (item == null) return;

        float weightToAdd = item.Weight * quantity;

        // Ensure the item can be added without exceeding weight limits
        if (!CanAddItem(weightToAdd))
        {
            ServiceLocator.Get<EventService>().OnTransactionFailed.Invoke("Weight limit exceeded!");
            return;
        }

        InventorySlot existingSlot = _slots.Find(slot => slot.Item == item); // Check if the item already exists in inventory

        if (existingSlot != null)
        {
            existingSlot.Quantity += quantity;
        }
        else
        {
            _slots.Add(new InventorySlot { Item = item, Quantity = quantity });
        }

        CurrentWeight += weightToAdd; // Update the current weight of the inventory

        ServiceLocator.Get<EventService>().OnInventoryUpdated.Invoke(); // Notify other systems that the inventory has been updated

    }

    // Removes an item from the inventory, ensuring there is enough quantity
    public void RemoveItem(ItemSO item, int quantity)
    {
        var slot = _slots.FirstOrDefault(s => s.Item == item);
        if (slot == null || slot.Quantity < quantity)
        {
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

    }

    // Checks if a new item can be added based on the current and maximum weight constraints.
    public bool CanAddItem(float weightToAdd)
    {
        return (CurrentWeight + weightToAdd) <= MaxWeight + WeightPrecision;
    }

    // Checks if the inventory contains a specific item with a required quantity
    public bool HasItem(ItemSO item, int quantity)
    {
        bool hasItem = _slots.Any(s => s.Item == item && s.Quantity >= quantity);
        return hasItem;
    }

    // Retrieves the current quantity of a specific item in the inventory.
    public int GetItemQuantity(ItemSO item)
    {
        var slot = _slots.Find(s => s.Item == item);
        return slot?.Quantity ?? 0;
    }

    // Calculates the total value of all items in the inventory based on their selling prices.
    private float CalculateTotalValue()
    {
        float total = 0f;
        foreach (var slot in _slots)
        {
            if (slot.Item != null)
            {
                total += slot.Item.SellingPrice * slot.Quantity;
            }
        }
        return total;
    }

    // Resets the inventory weight without removing items. Can be useful for temporary weight adjustments.
    public void ResetInventory()
    {
        CurrentWeight = 0f;
    }
}