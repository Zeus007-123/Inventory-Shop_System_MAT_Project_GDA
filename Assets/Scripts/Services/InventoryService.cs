using System.Collections.Generic;
using System.Linq;

public class InventoryService : IInventoryService
{
    public class InventorySlot
    {
        public ItemSO Item { get; set; }
        public int Quantity { get; set; }
    }

    private readonly List<InventorySlot> _slots = new();
    private readonly float _maxWeight;
    private float _currentWeight;

    // Explicit interface implementation
    IEnumerable<InventoryService.InventorySlot> IInventoryService.Slots => _slots.AsReadOnly();

    public float CurrentWeight => _currentWeight;
    public float MaxWeight => _maxWeight;
    public float TotalValue => _slots.Sum(s => s.Item.SellingPrice * s.Quantity);

    public InventoryService(float maxWeight) => _maxWeight = maxWeight;

    public void AddItem(ItemSO item, int quantity)
    {
        float totalWeight = item.Weight * quantity;
        if (!CanAddItem(totalWeight))
        {
            ServiceLocator.Get<EventService>().OnWeightLimitExceeded.Invoke();
            return;
        }

        var slot = _slots.FirstOrDefault(s => s.Item == item);
        if (slot != null) slot.Quantity += quantity;
        else _slots.Add(new InventorySlot { Item = item, Quantity = quantity });

        _currentWeight += totalWeight;
    }

    public void RemoveItem(ItemSO item, int quantity)
    {
        var slot = _slots.FirstOrDefault(s => s.Item == item);
        if (slot == null || slot.Quantity < quantity) return;

        slot.Quantity -= quantity;
        _currentWeight -= item.Weight * quantity;

        if (slot.Quantity <= 0)
            _slots.Remove(slot);
    }

    public bool CanAddItem(float weight) => _currentWeight + weight <= _maxWeight;
    public bool HasItem(ItemSO item, int quantity) =>
        _slots.Any(s => s.Item == item && s.Quantity >= quantity);
}