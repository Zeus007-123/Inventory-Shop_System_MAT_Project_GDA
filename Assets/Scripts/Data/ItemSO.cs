using UnityEngine;

/// <summary>
/// Represents an item in the game using a ScriptableObject.
/// Stores all the essential details about an item, including price, weight, and rarity.
/// </summary>
[CreateAssetMenu(fileName = "item", menuName = "scriptableObjects/item")]
public class ItemSO : ScriptableObject
{
    [Header("Item Properties")]
    [SerializeField] private Sprite sprite; // The visual representation of the item.
    [SerializeField] private ItemType itemType; // The type/category of the item.
    [SerializeField] private string itemName; // The name of the item.
    [SerializeField] private string description; // Description of the item.

    [Header("Item Values")]
    [SerializeField] private float buyingPrice; // Cost to purchase the item.
    [SerializeField] private float sellingPrice; // Value when selling the item.
    [SerializeField] private float weight; // The weight of the item, affecting inventory.

    [Header("Item Attributes")]
    [SerializeField] private ItemRarity itemRarity; // The rarity level of the item.
    [SerializeField] private int maxStackSize; // Maximum number of items that can be stacked together.

    // Public properties to access the private fields safely.
    public Sprite Sprite { get => sprite; }
    public ItemType ItemType { get => itemType; }
    public string ItemName { get => itemName; }
    public string Description { get => description; }
    public float BuyingPrice { get => buyingPrice; }
    public float SellingPrice { get => sellingPrice; }
    public float Weight { get => weight; }
    public ItemRarity ItemRarity { get => itemRarity; }
    public int MaxStackSize { get => maxStackSize; }

}