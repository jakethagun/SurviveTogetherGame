using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item_And_Quantity
{
    public Item item;
    public uint quantity;

    public Item_And_Quantity( Item inItem, uint inQuantity )
    {
        item = inItem;
        quantity = inQuantity;
    }
};

public class Storage_Manager : MonoBehaviour
{
    public List<Item_And_Quantity> storedItems = new List<Item_And_Quantity>();

    void Start()
    {
        PopulateItems();
    }

    void PopulateItems()
    {
        var allFiles = Resources.LoadAll<UnityEngine.Object>( "Items" );
        foreach( var obj in allFiles )
        {
            if( obj is GameObject )
            {
                GameObject go = obj as GameObject;
                if( go.GetComponent<Item>() != null )
                {
                    storedItems.Add( new Item_And_Quantity( go.GetComponent<Item>(), go.GetComponent<Item>().startGameQuantity ) );
                }
            }
        }
    }

    public Item_And_Quantity GetStoredItemAndQuantity( Item_And_Quantity item )
    {
        return GetStoredItemAndQuantity( item.item );
    }

    public Item_And_Quantity GetStoredItemAndQuantity( Item item )
    {
        foreach( Item_And_Quantity storedItem in storedItems )
        {
            if( storedItem.item.name == item.name )
            {
                return storedItem;
            }
        }

        throw new System.Exception( "Unable to find item requested" );
    }

    public void AddQuantity( Item_And_Quantity item )
    {
        foreach( Item_And_Quantity storedItem in storedItems )
        {
            if( storedItem.item.name == item.item.name )
            {
                storedItem.quantity += item.quantity;
                return;
            }
        }
    }

    public void SubtractQuantity( Item_And_Quantity item )
    {
        foreach( Item_And_Quantity storedItem in storedItems )
        {
            if( storedItem.item.name == item.item.name )
            {
                if( storedItem.quantity < item.quantity )
                {
                    throw new System.Exception( "Tried to subtract more quantity than available" );
                }

                storedItem.quantity -= item.quantity;
                return;
            }
        }
    }

    public List<Item_And_Quantity> GetStoredItems( Item.Category category, Item.Rarity rarity )
    {
        List<Item_And_Quantity> stored = new List<Item_And_Quantity>();

        foreach( Item_And_Quantity storedItem in storedItems )
        {
            if( storedItem.item.category == category && storedItem.item.rarity == rarity )
            {
                stored.Add( storedItem );
            }
        }

        return stored;
    }

    public List<Item_And_Quantity> GetStoredItems( Item.Category category )
    {
        List<Item_And_Quantity> stored = new List<Item_And_Quantity>();

        foreach( Item_And_Quantity storedItem in storedItems )
        {
            if( storedItem.item.category == category )
            {
                stored.Add( storedItem );
            }
        }

        return stored;
    }
}
