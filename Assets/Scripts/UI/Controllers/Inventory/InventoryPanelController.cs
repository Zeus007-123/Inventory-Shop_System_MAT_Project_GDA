/*using System.Collections.Generic;
using UnityEngine;

public class InventoryPanelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _inventorySlotPrefab;
    [SerializeField] private Transform _inventoryPanelParent; // Drag Inventory_Panel here

    private void Start()
    {
        var inventory = ServiceLocator.Get<InventoryService>();
        UpdateInventorySlots(inventory.Items);
    }

    public void UpdateInventorySlots(List<ItemSO> items)
    {
        // Clear existing slots
        foreach (Transform child in _inventoryPanelParent)
            Destroy(child.gameObject);

        // Create new slots
        foreach (ItemSO item in items)
        {
            GameObject slot = Instantiate(_inventorySlotPrefab, _inventoryPanelParent);
            slot.GetComponent<InventorySlotController>().Initialize(item, item.Quantity);
        }
    }
}*/