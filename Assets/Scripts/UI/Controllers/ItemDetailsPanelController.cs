/*using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.MPE;

public class ItemDetailsPanelController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _buyPrice;
    [SerializeField] private TextMeshProUGUI _sellPrice;
    [SerializeField] private TextMeshProUGUI _weight;
    [SerializeField] private TextMeshProUGUI _rarity;
    [SerializeField] private Button _actionButton;

    private void Start()
    {
        ServiceLocator.Get<EventService>().OnItemSelected.AddListener(ShowDetails);
        _panel.SetActive(false);
    }

    private void ShowDetails(ItemSO item, bool isFromShop)
    {
        _itemIcon.sprite = item.Sprite;
        _itemName.text = item.ItemName;
        _description.text = item.Description;
        _buyPrice.text = $"Buy: {item.BuyingPrice}G";
        _sellPrice.text = $"Sell: {item.SellingPrice}G";
        _weight.text = $"{item.Weight}kg";
        _rarity.text = item.ItemRarity.ToString();

        _actionButton.onClick.RemoveAllListeners();
        _actionButton.GetComponentInChildren<TextMeshProUGUI>().text = isFromShop ? "BUY" : "SELL";
        _actionButton.onClick.AddListener(() =>
        {
            ServiceLocator.Get<EventService>().OnTransactionInitiated.Trigger(item, isFromShop);
            _panel.SetActive(false);
        });

        _panel.SetActive(true);
    }
}*/