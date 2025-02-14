using System.Collections.Generic;
using UnityEngine;

public class ShopPanelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _shopSlotPrefab; // Prefab for individual shop item slots
    [SerializeField] private Transform _shopPanelParent; // Parent container for shop slots

    private void Start()
    {
        Debug.Log("ShopPanelController: Initializing shop panel...");

        var shopService = ServiceLocator.Get<ShopService>();

        if (shopService == null)
        {
            Debug.LogError("ShopPanelController: ShopService not found in ServiceLocator!");
            return;
        }

        InitializeShopSlots(shopService.AvailableItems);
    }

    /// <summary>
    /// Populates the shop UI with item slots.
    /// </summary>
    /// <param name="items">List of available items in the shop.</param>
    private void InitializeShopSlots(List<ItemSO> items)
    {
        if (items == null || items.Count == 0)
        {
            Debug.LogWarning("ShopPanelController: No items available in the shop!");
            return;
        }

        Debug.Log($"ShopPanelController: Initializing {items.Count} shop slots...");

        foreach (ItemSO item in items)
        {
            if (item == null)
            {
                Debug.LogError("ShopPanelController: Encountered a null item while creating shop slots!");
                continue;
            }

            GameObject slot = Instantiate(_shopSlotPrefab, _shopPanelParent);
            slot.GetComponent<ShopSlotController>().Initialize(item);

            Debug.Log($"ShopPanelController: Created shop slot for {item.ItemName}");
        }
    }
}
