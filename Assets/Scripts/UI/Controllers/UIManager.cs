/*using TMPro;
using UnityEditor.MPE;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _weightText;

    void Start()
    {
        var eventService = ServiceLocator.Get<EventService>();
        eventService.OnBuyTransaction.AddListener(UpdateUIAfterPurchase);
        eventService.OnSellTransaction.AddListener(UpdateUIAfterSale);
    }

    void UpdateUIAfterPurchase(ItemSO item, int quantity)
    {
        _coinsText.text = ServiceLocator.Get<CurrencyService>().CurrentCoins.ToString();
        _weightText.text = $"{ServiceLocator.Get<InventoryService>().CurrentWeight} / 1000";
    }
}*/