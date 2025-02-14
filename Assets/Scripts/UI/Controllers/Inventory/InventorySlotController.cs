using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotController : MonoBehaviour
{
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _quantityText;

    public void Initialize(ItemSO item, int quantity)
    {
        _itemIcon.sprite = item.Sprite;
        _quantityText.text = quantity.ToString();

        GetComponent<Button>().onClick.AddListener(() =>
            ServiceLocator.Get<EventService>().OnItemSelected.Invoke(item, false)
        );
    }
}