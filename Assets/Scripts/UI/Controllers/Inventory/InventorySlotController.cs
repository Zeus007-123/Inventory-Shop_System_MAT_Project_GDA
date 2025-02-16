using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _quantityText;
    [SerializeField] private Image _quantityBackground; // Background image for quantity display
    [SerializeField] private Button _itemButton;

    private ItemSO _currentItem;

    /*void Start()
    {
        _itemButton.onClick.AddListener(OnItemClicked);
    }*/

    public void Initialize(ItemSO item, int quantity)
    {
        _currentItem = item;
        _itemIcon.sprite = item.Sprite;
        _quantityBackground.gameObject.SetActive(true);
        //_quantityText.text = quantity.ToString();
        // Show quantity only if > 1
        _quantityText.text = quantity >= 1 ? quantity.ToString() : "";

        Debug.Log($"[InventorySlot] Created for {item.ItemName}");
    }

    public void OnItemClicked()
    {
        Debug.Log($"[InventorySlot] Selected item: {_currentItem.ItemName}");
        ServiceLocator.Get<EventService>().OnItemSelected?.Invoke(_currentItem, false); // false = from inventory
    }

}