using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the UI representation of an individual inventory slot.
/// Displays item icons, quantities, and handles user interactions.
/// </summary>

public class InventorySlotController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _quantityText;
    [SerializeField] private Image _quantityBackground; // Background image for quantity display
    [SerializeField] private Button _itemButton;

    private ItemSO _currentItem; // Stores the item associated with this slot
    private EventService _eventService;

    // Initializes references when the slot is instantiated. Retrieves the EventService via the ServiceLocator.
    private void Start()
    {
        _eventService = ServiceLocator.Get<EventService>();
    }

    // Sets up the slot with item data and updates the UI elements accordingly.
    public void Initialize(ItemSO item, int quantity)
    {
        _currentItem = item;
        _itemIcon.sprite = item.Sprite;
        _quantityBackground.gameObject.SetActive(true);

        // Show quantity only if >= 1
        _quantityText.text = quantity >= 1 ? quantity.ToString() : "";
    }

    // Called when the player clicks on an inventory item. Notifies the event system that an item was selected.
    public void OnItemClicked()
    {
        // Invoke event to notify that this item was selected from the inventory
        _eventService.OnItemSelected?.Invoke(_currentItem, false); // false = from inventory
    }

}