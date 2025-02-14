using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _weightText;

    void Start()
    {
        var eventService = ServiceLocator.Get<EventService>();
        eventService.OnBuyTransaction.AddListener(UpdateUI);
        eventService.OnSellTransaction.AddListener(UpdateUI);
    }

    void UpdateUI(ItemSO item, int quantity)
    {
        _coinsText.text = ServiceLocator.Get<CurrencyService>().CurrentCoins.ToString();
        _weightText.text = $"{ServiceLocator.Get<InventoryService>().CurrentWeight} / 1000";
    }
}