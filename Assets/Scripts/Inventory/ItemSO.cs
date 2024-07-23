using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "item", menuName = "scriptableObjects/item")]

public class ItemSO : ScriptableObject
{
    public Sprite sprite;
    public string itemName;
    public int maxStackSize;
    public ItemRarity itemRarity;
    public float buyingPrice;
    public float sellingPrice;
    public float weight;
    public string description;
    public ItemType itemType;

}