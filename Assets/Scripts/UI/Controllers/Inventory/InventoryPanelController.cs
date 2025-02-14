using System.Collections.Generic;
using UnityEngine;

public class InventoryPanelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _inventorySlotPrefab; // Prefab for inventory slot UI
    [SerializeField] private Transform _inventoryPanelParent; // Parent transform for inventory slots

    private void Start()
    {
        // Retrieve the inventory service instance from the Service Locator
        var inventory = ServiceLocator.Get<IInventoryService>();
        Debug.Log("InventoryPanelController: Retrieved Inventory Service");

        // Initialize UI slots based on current inventory items
        UpdateInventorySlots(inventory.Slots);
    }

    /// <summary>
    /// Updates the inventory UI slots based on the provided inventory data.
    /// </summary>
    /// <param name="slots">The list of inventory slots containing items.</param>
    public void UpdateInventorySlots(IEnumerable<InventoryService.InventorySlot> slots)
    {
        Debug.Log("Updating Inventory UI Slots...");

        // Clear existing inventory UI elements
        foreach (Transform child in _inventoryPanelParent)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("Cleared previous inventory slots.");

        // Generate new inventory slots based on the current inventory state
        foreach (var slot in slots)
        {
            GameObject slotObj = Instantiate(_inventorySlotPrefab, _inventoryPanelParent);
            slotObj.GetComponent<InventorySlotController>()
                   .Initialize(slot.Item, slot.Quantity);

            Debug.Log($"Created slot for {slot.Quantity}x {slot.Item.ItemName}");
        }

        Debug.Log("Inventory UI Slots Updated Successfully.");
    }
}
