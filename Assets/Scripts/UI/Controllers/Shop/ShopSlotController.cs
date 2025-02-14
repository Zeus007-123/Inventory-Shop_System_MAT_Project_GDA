using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
            // Pass arguments in the correct order without named parameters
            ServiceLocator.Get<EventService>().OnItemSelected.Invoke(_itemData, true);
        });
    }
}