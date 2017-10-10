// RATIONALE ------------------------------------
// * Manages the GUI related to the storage

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StorageManager : MonoBehaviour {

    // GUI --------------------------------------
    public Button storageButton;
    public Text itemText;

    // References -------------------------------
    public GameStateManager gameStateManager;

    // Storage ----------------------------------
    public List< Item_And_Quantity > storedItems = new List< Item_And_Quantity >();

	// Unity Callbacks -----------------------------
	void Start () 
    {
		storageButton.onClick.AddListener( OnStorageButtonClicked );

        // GameState
        gameStateManager.RegisterOnGameStateChanged( OnGameStateChanged );
        storageButton.gameObject.SetActive( false );

        PopulateItems();
	}

    public uint ItemCount()
    {
        uint itemCount = 0;
        foreach( var storedItem in storedItems )
        {
            itemCount += storedItem.quantity;
        }

        return itemCount;
    }

    public Item DropRandomItem()
    {
        List< int > storedItemsThatHaveQuantity = new List< int >();
        
        for( int i = 0; i < storedItems.Count; ++i )
        {
            if( storedItems[ i ].quantity > 0 )
            {
                storedItemsThatHaveQuantity.Add( i );
            }
        }

        int random = Random.Range( 0, storedItemsThatHaveQuantity.Count );

        Item_And_Quantity storedItem = storedItems[ random ];
        storedItem.quantity -= 1;
        storedItems[ random ] = storedItem;
        return storedItem.item;
    }

    public void AddItems( Item item, uint quantity )
    {
        for( int i = 0; i < storedItems.Count; ++i )
        {
            if( storedItems[ i ].item == item )
            {
                Item_And_Quantity stored = storedItems[ i ];
                stored.quantity += quantity;
                storedItems[ i ] = stored;
                break;
            }
        }
    }

    void PopulateItems()
    {
        var allFiles = Resources.LoadAll<UnityEngine.Object>( "Items" );
        foreach (var obj in allFiles)
        {
            if (obj is GameObject)
            {
                GameObject go = obj as GameObject;
                if ( go.GetComponent<Item>() != null )
                {
                    storedItems.Add( new Item_And_Quantity( go.GetComponent< Item >(), go.GetComponent< Item >().startGameQuantity ) );
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    // GUI --------------------------------------
    void OnGUI()
    {
        itemText.text = "";
        foreach( var storedItem in storedItems )
        {
            if( storedItem.quantity > 0 )
            {
                itemText.text += storedItem.quantity.ToString() + "x " + storedItem.item.name + '\n';
            }  
        }
    }

    void OnStorageButtonClicked()
    {
        if( gameStateManager.GetState() == GameState.State.STORAGE )
        {
            gameStateManager.ChangeState( GameState.State.DAY );
        }
        else
        {
            gameStateManager.ChangeState( GameState.State.STORAGE );
        }
    }

    // GameState --------------------------------
    void OnGameStateChanged( GameState.State newState, GameState.State previousState )
    {
        switch( newState )
        {
            case GameState.State.DAY:
                storageButton.gameObject.SetActive( true );
                itemText.gameObject.SetActive( false );  
                break;
            case GameState.State.STORAGE:
                itemText.gameObject.SetActive( true );             
                break;
            default:
                storageButton.gameObject.SetActive( false );
                break;
        }
    }
}
