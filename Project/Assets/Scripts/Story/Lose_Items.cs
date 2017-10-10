using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Lost_Item
{
    public Item itemToLose;

    public int minimumLost = 0;
    public int maximumLost = 1;
}

public class Lose_Items : MonoBehaviour
{
    public List<Lost_Item> items;
    public Text textToDisplayLostItems;
    
    void Start()
    {
        Storage_Manager storageManager = GameObject.Find( "Storage_Manager" ).GetComponent<Storage_Manager>();

        textToDisplayLostItems.text += "\n";

        foreach( Lost_Item lostItem in items )
        {
            Item_And_Quantity stored = storageManager.GetStoredItemAndQuantity( lostItem.itemToLose );
            Item_And_Quantity toLose = new Item_And_Quantity( stored.item, 0 );
            int randomAmount = Random.Range( lostItem.minimumLost, lostItem.maximumLost );

            if( stored.quantity > randomAmount )
            {
                toLose.quantity = ( uint )randomAmount;
                storageManager.SubtractQuantity( toLose );
            }
            else if( stored.quantity == 0 )
            {

            }
            else
            {
                // lose all
                storageManager.SubtractQuantity( stored );
            }

            textToDisplayLostItems.text += toLose.item.name + " x" + toLose.quantity.ToString() + '\n';
        }
    }
}
