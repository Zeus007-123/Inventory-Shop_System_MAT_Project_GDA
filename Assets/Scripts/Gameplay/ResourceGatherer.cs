/*using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.MPE;

public class ResourceGatherer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button _gatherButton;

    private void Start()
    {
        _gatherButton.onClick.AddListener(OnGatherResources);
    }

    private void OnGatherResources()
    {
        var inventory = ServiceLocator.Get<IInventoryService>();
        var eventService = ServiceLocator.Get<EventService>();

        // Weight Check
        if (inventory.CurrentWeight >= inventory.MaxWeight)
        {
            eventService.OnTransactionFailed.Trigger("Max weight reached!");
            eventService.OnPlaySound.Trigger(SoundTypes.FailedClick);
            return;
        }

        // Rarity Calculation
        ItemRarity rarity = CalculateDynamicRarity(inventory.TotalValue);
        ItemSO item = GetRandomItemByRarity(rarity);

        if (item != null)
        {
            inventory.AddItem(item, 1);
            eventService.OnPlaySound.Trigger(SoundTypes.SuccessfulClick);
        }
    }

    private ItemRarity CalculateDynamicRarity(float inventoryValue)
    {
        List<WeightedValue<ItemRarity>> weights = new()
        {
            new(ItemRarity.VeryCommon, Mathf.Max(40 - (0.1f * inventoryValue), 0)),
            new(ItemRarity.Common, Mathf.Max(30 - (0.05f * inventoryValue), 0)),
            new(ItemRarity.Rare, 20 + (0.1f * inventoryValue)),
            new(ItemRarity.Epic, 8 + (0.05f * inventoryValue)),
            new(ItemRarity.Legendary, 2 + (0.02f * inventoryValue))
        };

        return WeightedRandomizer.GetRandomItem(weights);
    }

    private ItemSO GetRandomItemByRarity(ItemRarity rarity)
    {
        var shopService = ServiceLocator.Get<ShopService>();
        List<ItemSO> candidates = shopService.AllItems
            .Where(item => item.ItemRarity == rarity)
            .ToList();

        return candidates.Count > 0 ?
            candidates[Random.Range(0, candidates.Count)] :
            null;
    }
}
*/