using UnityEngine;
using System.Linq;

public class InventoryPanelController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform _slotsParent;
    [SerializeField] private GameObject _slotPrefab;

    private IInventoryService _inventory;
    private EventService _eventService;

    void Start()
    {
        _inventory = ServiceLocator.Get<IInventoryService>();
        _eventService = ServiceLocator.Get<EventService>();

        _eventService.OnInventoryUpdated.AddListener(UpdateInventoryUI);
        UpdateInventoryUI();

        Debug.Log("[InventoryPanel] Initialized");
    }

    private void UpdateInventoryUI()
    {
        ClearSlots();

        foreach (InventorySlot slot in _inventory.Slots)
        {
            if (slot.Item == null) continue;

            GameObject slotObj = Instantiate(_slotPrefab, _slotsParent);
            //slotObj.GetComponent<InventorySlotController>().Initialize(slot.Item, slot.Quantity);
            var controller = slotObj.GetComponent<InventorySlotController>();
            controller.Initialize(slot.Item, slot.Quantity); // Pass quantity
        }

        Debug.Log($"[InventoryPanel] Updated with {_inventory.Slots.Count()} items");
    }

    private void ClearSlots()
    {
        foreach (Transform child in _slotsParent)
        {
            Destroy(child.gameObject);
        }
    }
}