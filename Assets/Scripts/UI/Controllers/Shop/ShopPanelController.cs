using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Manages the shop UI, including displaying items and filtering them based on categories.
/// It retrieves item data from the shop service and dynamically updates the UI.
/// </summary>

public class ShopPanelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _shopSlotsParent;
    [SerializeField] private GameObject _shopSlotPrefab;

    private IShopService _shopService; // Reference to the shop service handling item retrieval

    // Initializes the shop panel by fetching the shop service and loading all items.
    void Start()
    {
        _shopService = ServiceLocator.Get<IShopService>();
        LoadAllItems();
    }

    // Loads all available items in the shop.
    public void LoadAllItems() => LoadItems(ItemType.All);

    // Loads and displays shop items filtered by the specified item type.
    public void LoadItems(ItemType type)
    {
        ClearSlots(); // Remove existing shop item slots
        List<ItemSO> items = _shopService.GetItemsByCategory(type); // Retrieve items from the shop service based on the selected category

        // Create a UI slot for each retrieved item
        foreach (ItemSO item in items)
        {
            GameObject slotObj = Instantiate(_shopSlotPrefab, _shopSlotsParent);
            ShopSlotController slot = slotObj.GetComponent<ShopSlotController>();
            slot.Initialize(item); // Initialize the slot with item data
        }
    }

    // Handles shop category filter button clicks. Converts the category name to an ItemType and loads items accordingly.
    public void OnFilterButtonClicked(string categoryName)
    {
        if (Enum.TryParse(categoryName, out ItemType type)) // Convert category name to ItemType enum
        {
            LoadItems(type); // Load items for the selected category
        }
    }

    // Clears all shop item slots before loading new ones.
    private void ClearSlots()
    {
        foreach (Transform child in _shopSlotsParent)
        {
            if (child.CompareTag("ShopSlot")) // Ensures only shop slot prefabs are destroyed
                Destroy(child.gameObject);
        }
    }
}