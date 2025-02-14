using System.Collections.Generic;
using System.Linq;
using UnityEngine; 

public class InventoryService : IInventoryService
{
    // Represents an inventory slot holding an item and its quantity
    public class InventorySlot
    {
        public ItemSO Item { get; set; }
        public int Quantity { get; set; }
    }

    private readonly List<InventorySlot> _slots = new(); // Stores all inventory items
    private readonly float _maxWeight; // Maximum weight the inventory can hold
    private float _currentWeight; // Tracks the current weight of inventory

    // Exposes inventory slots as a read-only list (for interface implementation)
    IEnumerable<InventorySlot> IInventoryService.Slots => _slots.AsReadOnly();

    public float CurrentWeight => _currentWeight;
    public float MaxWeight => _maxWeight;
    public float TotalValue => _slots.Sum(s => s.Item.SellingPrice * s.Quantity);

    // Constructor initializes the inventory with a maximum weight limit
    public InventoryService(float maxWeight)
    {
        _maxWeight = maxWeight;
        Debug.Log($"InventoryService Initialized with max weight: {_maxWeight}");
    }

    // Adds an item to the inventory if there is enough capacity
    public void AddItem(ItemSO item, int quantity)
    {
        float totalWeight = item.Weight * quantity;
        if (!CanAddItem(totalWeight))
        {
            ServiceLocator.Get<EventService>().OnWeightLimitExceeded.Invoke();
            Debug.LogError($"Cannot add {quantity}x {item.ItemName}, weight limit exceeded!");
            return;
        }

        // Check if the item already exists in the inventory
        var slot = _slots.FirstOrDefault(s => s.Item == item);
        if (slot != null)
        {
            slot.Quantity += quantity;
        }
        else
        {
            _slots.Add(new InventorySlot { Item = item, Quantity = quantity });
        }

        _currentWeight += totalWeight;
        Debug.Log($"Added {quantity}x {item.ItemName} to inventory. Current Weight: {_currentWeight}/{_maxWeight}");
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
        _currentWeight -= item.Weight * quantity;

        // Remove slot if quantity reaches zero
        if (slot.Quantity <= 0)
        {
            _slots.Remove(slot);
        }

        Debug.Log($"Removed {quantity}x {item.ItemName} from inventory. Current Weight: {_currentWeight}/{_maxWeight}");
    }

    // Checks if an item can be added based on available weight capacity
    public bool CanAddItem(float weight)
    {
        bool canAdd = _currentWeight + weight <= _maxWeight;
        Debug.Log($"Can add item? {canAdd} (Current: {_currentWeight}, Max: {_maxWeight}, Item Weight: {weight})");
        return canAdd;
    }

    // Checks if the inventory contains a specific item with a required quantity
    public bool HasItem(ItemSO item, int quantity)
    {
        bool hasItem = _slots.Any(s => s.Item == item && s.Quantity >= quantity);
        Debug.Log($"Has {quantity}x {item.ItemName}? {hasItem}");
        return hasItem;
    }
}
