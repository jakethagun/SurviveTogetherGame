using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Scavengable_Item
{
    public Item scavengableItem;

    public int minimumScavengable = 0;
    public int maximumScavengable = 1;
    public int attempts = 1;
}

public class Scavenging_Location : MonoBehaviour
{
    public string description;
    public Sprite sprite;

    [SerializeField]
    List<Scavengable_Item> scavengableItems = new List<Scavengable_Item>();

    public List<Scavengable_Item> ScavengableItems()
    {
        return scavengableItems;
    }

    public List<Item_And_Quantity> Scavenge()
    {
        List<Item_And_Quantity> scavengedItems = new List<Item_And_Quantity>();

        foreach( Scavengable_Item item in scavengableItems )
        {
            int highest = 0;

            for( int attempt = 0; attempt < item.attempts; ++attempt)
            {
                int randomAmount = Random.Range( item.minimumScavengable, item.maximumScavengable );

                if( randomAmount > highest )
                {
                    highest = randomAmount;
                }
            }

            Item_And_Quantity scavengedItem = new Item_And_Quantity( item.scavengableItem, (uint)highest );
            scavengedItems.Add( scavengedItem );
        }

        return scavengedItems;
    }
}
