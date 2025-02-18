using UnityEngine;
using UnityEngine.UI;

public class ShopSlotController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image _itemIcon; // UI element for displaying item sprite
    [SerializeField] private Button _itemButton; // Button to handle item selection

    private EventService _eventService;

    //private ItemSO _itemData; // Stores the item data associated with this shop slot

    void Start()
    {
        _eventService = ServiceLocator.Get<EventService>();
    }

    /// <summary>
    /// Initializes the shop slot with item data.
    /// </summary>
    /// <param name="item">The item to display in this slot.</param>
    public void Initialize(ItemSO item)
    {
        if (item == null)
        {
            Debug.LogError("ShopSlotController: Item data is null. Cannot initialize shop slot.");
            return;
        }

        //_itemData = item;
        _itemIcon.sprite = item.Sprite;

        // Remove existing listeners to avoid duplicate event bindings
        _itemButton.onClick.RemoveAllListeners();
        _itemButton.onClick.AddListener(() =>
        {
            Debug.Log($"[ShopSlot] Clicked item: {item.ItemName}");

            // Trigger item selection event with correct order of arguments
            _eventService.OnItemSelected?.Invoke(item, true);
        });

        Debug.Log($"ShopSlotController: Initialized shop slot for {item.ItemName} with price {item.BuyingPrice}G.");
    }
}
