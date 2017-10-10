using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengingLocation : MonoBehaviour {

    // create a scavenge location GO which has the
    // items that can be scavenged along with the chance
            // of scavenging that

    [System.Serializable]
    public class Scavengable_Item
    {
       public Item scavengableItem;

       public uint minimumScavengable = 0;
       public uint maximumScavengable = 1;
    }

    public List< Scavengable_Item > scavengableItems = new List< Scavengable_Item >();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public class ScavengeResult
    {
       public ScavengingLocation location;
       public List< Item_And_Quantity > scavengedItems = new List< Item_And_Quantity >();

        public ScavengeResult( ScavengingLocation inLocation )
        {
            location = inLocation;
        }

        public void PrintResult()
        {
           Debug.Log( "Scavenged location : " + location.name );
           foreach( var scavengedItem in scavengedItems )
           {
                Debug.Log( "Scavenged : " + scavengedItem.item.name + " x" + scavengedItem.quantity );
            }
        }
    }

    public ScavengeResult Scavenge()
    {
        ScavengeResult result = new ScavengeResult( this );

        foreach( var item in scavengableItems )
        {
            int random = Random.Range( (int)item.minimumScavengable, (int)item.maximumScavengable + 1 );
            if( random > 0 )
            {
                StorageManager storageManager = GameObject.Find( "StorageManager" ).GetComponent< StorageManager >();
                storageManager.AddItems( item.scavengableItem, (uint)random );
            
                Item_And_Quantity scavenged = new Item_And_Quantity( item.scavengableItem, (uint)random );
                result.scavengedItems.Add( scavenged );
            }
        }

        return result;
    }
}
