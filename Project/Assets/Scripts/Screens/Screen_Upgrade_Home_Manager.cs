using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Upgrade_Home_Manager : MonoBehaviour
{
    private GameState_Manager gameStateManager;
    public Transform itemPanelParent;
    public Transform perkPanelParent;

    Home_Manager homeManager;
    Storage_Manager storageManager;

    List<Item_And_Quantity> itemsRequiredToUpgrade;
    List<Item_And_Quantity> storedItems;
    bool canUpgrade = false;

    List<GameObject> itemScreens = new List<GameObject>();
    public GameObject itemTemplate;

    List<GameObject> perkScreens = new List<GameObject>();
    public GameObject perkTemplate;

    public Button upgradeButton;
    public Text homeLevelText;

    // Use this for initialization
    void Awake()
    {
        homeManager = GameObject.Find( "Home_Manager" ).GetComponent<Home_Manager>();
        storageManager = GameObject.Find( "Storage_Manager" ).GetComponent<Storage_Manager>();

        gameStateManager = GameObject.Find( "GameState_Manager" ).GetComponent<GameState_Manager>();

        upgradeButton.onClick.AddListener( OnClickedUpgrade );
    }

    void OnClickedUpgrade()
    {
        Upgrade();
    }

    void RefreshScreen()
    {
        // Clear screens
        foreach( GameObject itemScreen in itemScreens )
        {
            GameObject.Destroy( itemScreen );
        }
        itemScreens.Clear();

        foreach( GameObject perkScreen in perkScreens )
        {
            GameObject.Destroy( perkScreen );
        }
        perkScreens.Clear();

        // Level
        int homeLevel = homeManager.CurrentLevel();

        if( homeManager.HasUpgradeAvailable() )
        {
            homeLevelText.text = "HOME UPGRADE " + ( homeLevel + 1 ).ToString();

            canUpgrade = true;

            storedItems = new List<Item_And_Quantity>();
            itemsRequiredToUpgrade = homeManager.GetRequiredItemsToUpgrade();
            foreach( Item_And_Quantity requiredItemAndQuantity in itemsRequiredToUpgrade )
            {
                // Get stored item
                Item_And_Quantity storedItem = storageManager.GetStoredItemAndQuantity( requiredItemAndQuantity );
                storedItems.Add( storedItem );

                // Add item to GUI
                GameObject newItemScreen = GameObject.Instantiate( itemTemplate );
                newItemScreen.transform.SetParent( itemPanelParent, false );
                itemScreens.Add( newItemScreen );

                Screen_Item_Manager itemScreen = newItemScreen.GetComponent<Screen_Item_Manager>();
                itemScreen.nameText.text = requiredItemAndQuantity.item.name;
                itemScreen.quantityText.text = storedItem.quantity.ToString() + " / " + requiredItemAndQuantity.quantity.ToString();
                itemScreen.image.sprite = requiredItemAndQuantity.item.storageSprite;

                // Determine if can upgrade
                if( storedItem.quantity < requiredItemAndQuantity.quantity )
                {
                    canUpgrade = false;
                    itemScreen.quantityText.color = Color.red;
                }
                else
                {
                    itemScreen.quantityText.color = Color.green;
                }
            }

            // Add perks
            List<string> perks = homeManager.GetPerksOfUpgrade();

            foreach( string perk in perks )
            {
                GameObject newPerkScreen = GameObject.Instantiate( perkTemplate, perkPanelParent, false );
                perkScreens.Add( newPerkScreen );

                Screen_Perk_Manager perkScreenManager = newPerkScreen.GetComponent<Screen_Perk_Manager>();
                perkScreenManager.perkText.text = perk;
            }
        }
        else
        {
            homeLevelText.text = "MAX HOME LEVEL REACHED";
            canUpgrade = false;
            upgradeButton.gameObject.SetActive( false );
        }

        if( canUpgrade )
        {
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeButton.interactable = false;
            
        }
    }

    public void ShowScreen()
    {
        gameObject.SetActive( true );

        RefreshScreen();
    }

    void Upgrade()
    {
        homeManager.Upgrade();
        RefreshScreen();
    }

    public void HideScreen()
    {
        gameObject.SetActive( false );
    }

}
