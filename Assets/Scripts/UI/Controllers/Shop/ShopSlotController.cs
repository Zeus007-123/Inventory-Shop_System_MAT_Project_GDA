using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls an individual shop slot in the UI.
/// Handles displaying item data and managing item selection.
/// </summary>

public class ShopSlotController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image _itemIcon; 
    [SerializeField] private Button _itemButton; 

    private EventService _eventService;

    // Initializes the shop slot controller by retrieving the event service.
    void Start()
    {
        _eventService = ServiceLocator.Get<EventService>();
    }

    // Sets up the shop slot with item data and configures the selection button.
    public void Initialize(ItemSO item)
    {
        if (item == null) return; // Ensure item is valid before proceeding

        _itemIcon.sprite = item.Sprite;

        // Remove any previous listeners to prevent duplicate event triggers
        _itemButton.onClick.RemoveAllListeners();
        // Add a new listener to handle item selection when the button is clicked
        _itemButton.onClick.AddListener(() =>
        {
            // Notify event service that this item has been selected from the shop
            _eventService.OnItemSelected?.Invoke(item, true);
        });

    }
}
