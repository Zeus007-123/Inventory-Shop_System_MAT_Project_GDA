/*using System.Collections.Generic;
using UnityEditor.MPE;

public class InventoryService : IInventoryService
{
    private readonly float _maxWeight;
    private float _currentWeight;
    private List<ItemSO> _items = new();

    public float TotalValue => _items.Sum(item => item.SellingPrice * quantity);
    public bool IsFull => CurrentWeight >= MaxWeight;

    public bool CanAddItem(float weight) => CurrentWeight + weight <= MaxWeight;
    public bool HasItem(ItemSO item, int quantity) =>
    _items.Count(i => i == item) >= quantity;


    public InventoryService(float maxWeight) => _maxWeight = maxWeight;

    public void AddItem(ItemSO item, int quantity)
    {
        float totalWeight = item.Weight * quantity;
        if (_currentWeight + totalWeight > _maxWeight)
        {
            ServiceLocator.Get<EventService>().OnWeightLimitExceeded.Trigger();
            return;
        }

        // Add item logic
        _currentWeight += totalWeight;
        ServiceLocator.Get<EventService>().OnPlaySound.Trigger(SoundTypes.SuccessfulClick);
        ServiceLocator.Get<InventoryPanelController>().UpdateInventorySlots(_items);
    }

    public void RemoveItem(ItemSO item, int quantity)
    {
        // Remove item logic
        _currentWeight -= item.Weight * quantity;
        ServiceLocator.Get<EventService>().OnPlaySound.Trigger(SoundTypes.SuccessfulClick);
    }
}*/