using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image _itemIcon; // UI image component for item icon
    [SerializeField] private TextMeshProUGUI _quantityText; // UI text component for item quantity
    [SerializeField] private Image _quantityBackground; // Background image for quantity display

    /// <summary>
    /// Initializes the inventory slot with the given item data.
    /// </summary>
    /// <param name="item">The item data (ScriptableObject).</param>
    /// <param name="quantity">The quantity of the item.</param>
    public void Initialize(ItemSO item, int quantity)
    {
        if (item == null)
        {
            Debug.LogError("InventorySlotController: Attempted to initialize with a null item!");
            return;
        }

        // Assign the item sprite and quantity to the UI elements
        _itemIcon.sprite = item.Sprite;
        _quantityText.text = quantity.ToString();

        // Enable and set the quantity background
        _quantityBackground.gameObject.SetActive(quantity > 1);

        Debug.Log($"InventorySlot Initialized - Item: {item.ItemName}, Quantity: {quantity}");

        // Add a button click listener to handle item selection
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log($"Item Selected: {item.ItemName}");
            ServiceLocator.Get<EventService>().OnItemSelected.Invoke(item, false);
        });
    }
}
