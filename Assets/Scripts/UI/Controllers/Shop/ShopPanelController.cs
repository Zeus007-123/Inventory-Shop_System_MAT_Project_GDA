using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

/// <summary>
/// Controls the shop UI, including item display and filtering.
/// </summary>
public class ShopPanelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _shopSlotsParent;
    [SerializeField] private GameObject _shopSlotPrefab;

    private IShopService _shopService;
    private EventService _eventService;

    void Start()
    {
        _shopService = ServiceLocator.Get<IShopService>();
        _eventService = ServiceLocator.Get<EventService>();

        LoadAllItems();
        Debug.Log("[ShopPanel] Initialized shop UI.");
    }

    public void LoadAllItems() => LoadItems(ItemType.All);

    public void LoadItems(ItemType type)
    {
        ClearSlots();
        List<ItemSO> items = _shopService.GetItemsByCategory(type);

        Debug.Log($"[ShopPanel] Loading {items.Count} items for category: {type}");

        foreach (ItemSO item in items)
        {
            GameObject slotObj = Instantiate(_shopSlotPrefab, _shopSlotsParent);
            ShopSlotController slot = slotObj.GetComponent<ShopSlotController>();
            slot.Initialize(item);
        }
    }

    // Add this method for button clicks
    public void OnFilterButtonClicked(string categoryName)
    {
        ItemType type;
        if (Enum.TryParse(categoryName, out type))
        {
            LoadItems(type);
        }
        else
        {
            Debug.LogError($"Invalid category: {categoryName}");
        }
    }

    private void ClearSlots()
    {
        foreach (Transform child in _shopSlotsParent)
        {
            if (child.CompareTag("ShopSlot")) // Add tag to slot prefabs
                Destroy(child.gameObject);
        }
        Debug.Log("[ShopPanel] Cleared existing slots.");
    }
}