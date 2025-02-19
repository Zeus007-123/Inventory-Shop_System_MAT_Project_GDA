using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Controls the UI panel that displays detailed information about an item.
/// This panel is shown when an item is selected in the inventory or shop.
/// </summary>

public class ItemDetailsPanelController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _panel; // The UI panel that displays item details
    [SerializeField] private Image _itemIcon; // Image component for displaying the item sprite
    [SerializeField] private TextMeshProUGUI _itemType; // UI text for item type
    [SerializeField] private TextMeshProUGUI _itemName; // UI text for the item name
    [SerializeField] private TextMeshProUGUI _description; // UI text for the item description
    [SerializeField] private TextMeshProUGUI _buyPrice; // UI text displaying item buy price
    [SerializeField] private TextMeshProUGUI _sellPrice; // UI text displaying item sell price
    [SerializeField] private TextMeshProUGUI _weight; // UI text for item weight
    [SerializeField] private TextMeshProUGUI _rarity; // UI text for item rarity
    [SerializeField] private TextMeshProUGUI _maxStackSize; // UI text for max stack size
    [SerializeField] private Button _actionButton; // Button used for Buy/Sell actions

    //Initializes the panel by subscribing to the item selection event. Ensures the panel is hidden at the start.
    private void Start()
    {
        // Subscribe to the item selection event to show item details
        ServiceLocator.Get<EventService>().OnItemSelected.AddListener(ShowDetails);
        _panel.SetActive(false); // Hide panel initially
    }

    // Displays detailed information about a selected item.
    public void ShowDetails(ItemSO item, bool isFromShop)
    {
        _panel.SetActive(true); // Show the panel

        if (item == null) return; // Prevent null reference errors

        // Set UI elements with item details
        _itemIcon.sprite = item.Sprite;
        _itemType.text = item.ItemType.ToString();
        _itemName.text = item.ItemName;
        _description.text = item.Description;
        _buyPrice.text = $"${item.BuyingPrice}";
        _sellPrice.text = $"${item.SellingPrice}";
        _weight.text = $"{item.Weight}";
        _rarity.text = item.ItemRarity.ToString();
        _maxStackSize.text = item.MaxStackSize.ToString();

        // Configure the action button depending on whether the item is from the shop or inventory
        _actionButton.GetComponentInChildren<TextMeshProUGUI>().text = isFromShop ? "BUY" : "SELL";
        // Remove any previous listeners to prevent multiple calls
        _actionButton.onClick.RemoveAllListeners();
        // Add a new listener to trigger the transaction process
        _actionButton.onClick.AddListener(() =>
        {
            ServiceLocator.Get<EventService>().OnTransactionInitiated?.Invoke(item, isFromShop);
            _panel.SetActive(false);// Hide the panel after action
        });

    }

    // Hides the item details panel.
    public void HideDetails()
    { 
        _panel.SetActive(false);
    }
}
