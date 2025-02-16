using UnityEngine;
using UnityEngine.UI;

public class ShopSlotController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image _itemIcon; // UI element for displaying item sprite
    //[SerializeField] private TextMeshProUGUI _priceText; // UI element for displaying item price
    [SerializeField] private Button _itemButton; // Button to handle item selection

    private ItemSO _itemData; // Stores the item data associated with this shop slot

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

        _itemData = item;
        _itemIcon.sprite = item.Sprite;
        //_priceText.text = $"{item.BuyingPrice}G"; // Display item price in gold (G)

        // Remove existing listeners to avoid duplicate event bindings
        _itemButton.onClick.RemoveAllListeners();
        _itemButton.onClick.AddListener(() =>
        {
            Debug.Log($"[ShopSlot] Clicked item: {item.ItemName}");

            // Trigger item selection event with correct order of arguments
            ServiceLocator.Get<EventService>().OnItemSelected?.Invoke(item, true);
        });

        Debug.Log($"ShopSlotController: Initialized shop slot for {item.ItemName} with price {item.BuyingPrice}G.");
    }
}
