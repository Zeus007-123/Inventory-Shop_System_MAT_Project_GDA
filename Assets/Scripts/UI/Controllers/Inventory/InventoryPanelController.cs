using System.Collections.Generic;
using UnityEngine;

public class InventoryPanelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _inventorySlotPrefab;
    [SerializeField] private Transform _inventoryPanelParent;

    private void Start()
    {
        var inventory = ServiceLocator.Get<IInventoryService>();
        UpdateInventorySlots(inventory.Slots);
    }

    public void UpdateInventorySlots(IEnumerable<InventoryService.InventorySlot> slots)
    {
        foreach (Transform child in _inventoryPanelParent)
            Destroy(child.gameObject);

        foreach (var slot in slots)
        {
            GameObject slotObj = Instantiate(_inventorySlotPrefab, _inventoryPanelParent);
            slotObj.GetComponent<InventorySlotController>()
                   .Initialize(slot.Item, slot.Quantity);
        }
    }
}