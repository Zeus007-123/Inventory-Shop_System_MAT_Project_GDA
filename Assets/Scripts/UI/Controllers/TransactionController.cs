using UnityEngine;
using TMPro;

public class TransactionController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _transactionPanel;
    [SerializeField] private TMP_Text _itemNameText;
    [SerializeField] private TMP_Text _quantityText;
    [SerializeField] private TMP_Text _totalPriceText;

    private ItemSO _currentItem;
    private int _currentQuantity = 1;
    private TransactionType _currentTransactionType;

    private void Start()
    {
        ServiceLocator.Get<EventService>().OnItemSelected.AddListener(HandleItemSelection);
    }

    private void HandleItemSelection(ItemSO item, bool isFromShop)
    {
        _currentItem = item;
        _currentTransactionType = isFromShop ? TransactionType.Buy : TransactionType.Sell;
        ResetTransactionUI();
        _transactionPanel.SetActive(true);
    }

    private void ResetTransactionUI()
    {
        _currentQuantity = 1;
        UpdateUI();
    }

    public void IncreaseQuantity()
    {
        _currentQuantity = Mathf.Clamp(_currentQuantity + 1, 1, _currentItem.MaxStackSize);
        UpdateUI();
    }

    public void DecreaseQuantity()
    {
        _currentQuantity = Mathf.Clamp(_currentQuantity - 1, 1, _currentItem.MaxStackSize);
        UpdateUI();
    }

    private void UpdateUI()
    {
        _itemNameText.text = _currentItem.ItemName;
        _quantityText.text = _currentQuantity.ToString();

        float pricePerUnit = _currentTransactionType == TransactionType.Buy ?
            _currentItem.BuyingPrice :
            _currentItem.SellingPrice;

        _totalPriceText.text = $"Total: {pricePerUnit * _currentQuantity}G";
    }

    public void ConfirmTransaction()
    {
        ServiceLocator.Get<TransactionService>().ProcessTransaction(
            new TransactionData
            {
                Item = _currentItem,
                Quantity = _currentQuantity,
                Type = _currentTransactionType
            }
        );
        _transactionPanel.SetActive(false);
    }
}