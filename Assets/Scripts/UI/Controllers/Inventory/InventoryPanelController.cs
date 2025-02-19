using UnityEngine;

/// <summary>
/// Manages the inventory panel UI by displaying the player's inventory items dynamically.
/// Listens for inventory updates and refreshes the UI accordingly.
/// </summary>

public class InventoryPanelController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform _slotsParent;
    [SerializeField] private GameObject _slotPrefab;

    private IInventoryService _inventory;
    private EventService _eventService;

    // Initializes the inventory panel by setting up dependencies and subscribing to events.
    void Start()
    {
        _inventory = ServiceLocator.Get<IInventoryService>();
        _eventService = ServiceLocator.Get<EventService>();

        // Subscribe to the inventory update event to refresh UI when inventory changes
        _eventService.OnInventoryUpdated.AddListener(UpdateInventoryUI);
        // Initial UI update to display current inventory items
        UpdateInventoryUI();
    }

    // Updates the inventory UI by clearing old slots and creating new ones based on the current inventory state.
    private void UpdateInventoryUI()
    {
        if (_slotsParent == null)
        {
            return;
        }

        // Clear existing inventory slots before updating
        ClearSlots();

        // Populate the inventory panel with items from the inventory
        foreach (InventorySlot slot in _inventory.Slots)
        {
            if (slot.Item == null) continue;

            GameObject slotObj = Instantiate(_slotPrefab, _slotsParent);
            slotObj.GetComponent<InventorySlotController>().Initialize(slot.Item, slot.Quantity);
        }
    }

    // Clears all existing UI elements inside the inventory panel before refreshing it. Prevents duplication of UI elements.
    private void ClearSlots()
    {
        foreach (Transform child in _slotsParent)
        {
            Destroy(child.gameObject);
        }
    }
}