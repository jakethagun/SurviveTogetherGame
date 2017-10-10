using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scavenging_Manager : MonoBehaviour
{
    Storage_Manager storageManager;

    List<Item_And_Quantity> previousScavengeItems = new List<Item_And_Quantity>();

    private void Awake()
    {
        storageManager = GameObject.Find( "Storage_Manager" ).GetComponent<Storage_Manager>();
    }

    public void Scavenge( Scavenging_Location location )
    {
        previousScavengeItems = location.Scavenge();

        foreach( Item_And_Quantity scavengedItem in previousScavengeItems )
        {
            storageManager.AddQuantity( scavengedItem );
        }
    }

    public List<Item_And_Quantity> PreviousScavengedItems()
    {
        return previousScavengeItems;
    }
}
