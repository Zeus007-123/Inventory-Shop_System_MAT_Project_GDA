/*using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.MPE;

public class InventorySlotController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _quantityText;

    private ItemSO _itemData;

    public void Initialize(ItemSO item, int quantity)
    {
        _itemData = item;
        _itemIcon.sprite = item.Sprite;
        _quantityText.text = quantity.ToString();

        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() =>
        {
            ServiceLocator.Get<EventService>().OnItemSelected.Trigger(_itemData, isFromShop: false);
        });
    }
}*/