using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Required_Materials_To_Craft
{
    public List<Item_And_Quantity> requiredItems;
}

public class Item : MonoBehaviour
{
    public enum Category { HOME, FARM, WATER_COLLECTION, WEAPONS, MEDICAL, TRADING_OBJECT, FOOD };
    public enum Rarity { COMMON, UNCOMMON, RARE, ULTRA_RARE };

    public Category category;
    public Rarity rarity;
    public uint startGameQuantity = 0;
    public Sprite storageSprite;
    public Required_Materials_To_Craft requiredMaterialsToCraft;
}
