/*using System.Collections.Generic;
using UnityEngine;

public class ShopPanelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _shopSlotPrefab;
    [SerializeField] private Transform _shopPanelParent; // Drag Shop_Panel here

    private void Start()
    {
        var shopService = ServiceLocator.Get<ShopService>();
        InitializeShopSlots(shopService.AvailableItems);
    }

    private void InitializeShopSlots(List<ItemSO> items)
    {
        foreach (ItemSO item in items)
        {
            GameObject slot = Instantiate(_shopSlotPrefab, _shopPanelParent);
            slot.GetComponent<ShopSlotController>().Initialize(item);
        }
    }
}*/