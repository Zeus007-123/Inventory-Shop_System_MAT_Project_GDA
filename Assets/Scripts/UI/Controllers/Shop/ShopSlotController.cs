/*using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.MPE;

public class ShopSlotController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Button _itemButton;

    private ItemSO _itemData;

    public void Initialize(ItemSO item)
    {
        _itemData = item;
        _itemIcon.sprite = item.Sprite;
        _priceText.text = $"{item.BuyingPrice}G";

        _itemButton.onClick.RemoveAllListeners();
        _itemButton.onClick.AddListener(() =>
        {
            ServiceLocator.Get<EventService>().OnItemSelected.Trigger(_itemData, isFromShop: true);
        });
    }
}*/