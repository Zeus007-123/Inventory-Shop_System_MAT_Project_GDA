using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "item", menuName = "scriptableObjects/item")]

public class ItemSO : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private ItemType itemType;
    [SerializeField] private string itemName;
    [SerializeField] private string description;
    [SerializeField] private float buyingPrice;
    [SerializeField] private float sellingPrice;
    [SerializeField] private float weight;
    [SerializeField] private ItemRarity itemRarity;
    [SerializeField] private int maxStackSize;
    
    public Sprite Sprite { get => sprite; }
    public ItemType ItemType { get => itemType; }
    public string ItemName { get => itemName;}
    public string Description { get => description;}
    public float BuyingPrice { get => buyingPrice;}
    public float SellingPrice { get => sellingPrice;}
    public float Weight { get => weight;}
    public ItemRarity ItemRarity { get => itemRarity;}

    
    
    

}