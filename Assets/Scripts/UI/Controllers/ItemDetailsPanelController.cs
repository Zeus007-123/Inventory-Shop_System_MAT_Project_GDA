using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemDetailsPanelController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _panel; // The UI panel that displays item details
    [SerializeField] private Image _itemIcon; // Image component for displaying the item sprite
    [SerializeField] private TextMeshProUGUI _itemName; // UI text for the item name
    [SerializeField] private TextMeshProUGUI _description; // UI text for the item description
    [SerializeField] private TextMeshProUGUI _buyPrice; // UI text displaying item buy price
    [SerializeField] private TextMeshProUGUI _sellPrice; // UI text displaying item sell price
    [SerializeField] private TextMeshProUGUI _weight; // UI text for item weight
    [SerializeField] private TextMeshProUGUI _rarity; // UI text for item rarity
    [SerializeField] private TextMeshProUGUI _maxStackSize; // UI text for max stack size
    [SerializeField] private Button _actionButton; // Button used for Buy/Sell actions

    private void Start()
    {
        // Subscribe to the item selection event to show item details
        ServiceLocator.Get<EventService>().OnItemSelected.AddListener(ShowDetails);
        _panel.SetActive(false); // Hide panel initially
        Debug.Log("ItemDetailsPanelController: Initialized and panel set to inactive.");
    }

    /// <summary>
    /// Displays item details in the UI panel when an item is selected.
    /// </summary>
    /// <param name="item">The item to display.</param>
    /// <param name="isFromShop">True if the item is from the shop, false if from inventory.</param>
    private void ShowDetails(ItemSO item, bool isFromShop)
    {
        if (item == null)
        {
            Debug.LogWarning("ItemDetailsPanelController: ShowDetails called with a null item.");
            return;
        }

        // Set UI elements with item details
        _itemIcon.sprite = item.Sprite;
        _itemName.text = item.ItemName;
        _description.text = item.Description;
        _buyPrice.text = $"Buy: {item.BuyingPrice}G";
        _sellPrice.text = $"Sell: {item.SellingPrice}G";
        _weight.text = $"{item.Weight}kg";
        _rarity.text = item.ItemRarity.ToString();
        _maxStackSize.text = item.MaxStackSize.ToString();

        // Configure action button for either buying or selling
        _actionButton.onClick.RemoveAllListeners();
        _actionButton.GetComponentInChildren<TextMeshProUGUI>().text = isFromShop ? "BUY" : "SELL";

        _actionButton.onClick.AddListener(() =>
        {
            Debug.Log($"ItemDetailsPanelController: {(isFromShop ? "Buying" : "Selling")} {item.ItemName}");
            ServiceLocator.Get<EventService>().OnTransactionInitiated?.Invoke(item, isFromShop);
            _panel.SetActive(false);
        });

        _panel.SetActive(true);
        Debug.Log($"ItemDetailsPanelController: Displaying details for {item.ItemName}, from {(isFromShop ? "Shop" : "Inventory")}");
    }
}
