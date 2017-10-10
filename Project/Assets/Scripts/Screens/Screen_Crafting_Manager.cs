using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Crafting_Manager : MonoBehaviour
{
    public Button[] categoryButtons;
    public Text[] categoryButtonsText;

    public Button[] rarityButtons;
    public Text[] rarityButtonsText;

    Item.Category currentCategory = Item.Category.HOME;
    Item.Rarity currentRarity = Item.Rarity.COMMON;

    public Button backButton;

    private GameState_Manager gameStateManager;
    Storage_Manager storageManager;

    Button selectedCategoryButton;
    Text selectedCategoryText;

    Button selectedRarityButton;
    Text selectedRarityText;

    List<Item_And_Quantity> storedItemsOfCategoryAndRarity = new List<Item_And_Quantity>();

    public Transform itemPanelParent;
    public GameObject itemTemplate;
    List<GameObject> itemScreens = new List<GameObject>();

    public Button craftButton;
    public GameObject scavengeOnlyDialog;
    public GameObject craftDialog;
    public Transform craftMaterialRequirementsParent;
    List<GameObject> requiredCraftItemsScreens = new List<GameObject>();

    Item selectedItemToCraft;

    // Use this for initialization
    void Awake()
    {
        backButton.onClick.AddListener( OnBackButtonClicked );

        gameStateManager = GameObject.Find( "GameState_Manager" ).GetComponent<GameState_Manager>();
        storageManager = GameObject.Find( "Storage_Manager" ).GetComponent<Storage_Manager>();

        selectedCategoryButton = categoryButtons[0];
        selectedCategoryText = categoryButtonsText[0];

        selectedRarityButton = rarityButtons[0];
        selectedRarityText = rarityButtonsText[0];

        for( int categoryButtonIndex = 0; categoryButtonIndex < categoryButtons.Length; ++categoryButtonIndex )
        {
            int captured = categoryButtonIndex;
            categoryButtons[categoryButtonIndex].onClick.AddListener( () => OnClickCategoryButton( captured ) );
        }

        for( int rarityButtonIndex = 0; rarityButtonIndex < rarityButtons.Length; ++rarityButtonIndex )
        {
            int captured = rarityButtonIndex;
            rarityButtons[rarityButtonIndex].onClick.AddListener( () => OnClickRarityButton( captured ) );
        }

        craftButton.onClick.AddListener( OnClickCraft );
    }

    void OnClickCategoryButton( int index )
    {
        currentCategory = ( Item.Category )index;

        Button previouslySelectedCategoryButton = selectedCategoryButton;
        Text previouslySelectedCategoryText = selectedCategoryText;

        selectedCategoryButton = categoryButtons[index];
        selectedCategoryText = categoryButtonsText[index];

        ColorBlock unselectedColors = selectedCategoryButton.colors;
        unselectedColors.normalColor = Color.white;
        unselectedColors.highlightedColor = Color.gray;
        previouslySelectedCategoryButton.colors = unselectedColors;
        previouslySelectedCategoryText.color = Color.black;

        ColorBlock selectedColors = selectedCategoryButton.colors;
        selectedColors.normalColor = Color.black;
        selectedColors.highlightedColor = Color.black;
        selectedCategoryButton.colors = selectedColors;
        selectedCategoryText.color = Color.white;

        RepopulateItems();

        craftDialog.SetActive( false );
        scavengeOnlyDialog.SetActive( false );
    }

    void OnClickRarityButton( int index )
    {
        currentRarity = ( Item.Rarity )index;

        Button previouslySelectedRarityButton = selectedRarityButton;
        Text previouslySelectedRarityText = selectedRarityText;

        selectedRarityButton = rarityButtons[index];
        selectedRarityText = rarityButtonsText[index];

        ColorBlock unselectedColors = selectedRarityButton.colors;
        unselectedColors.normalColor = Color.white;
        unselectedColors.highlightedColor = Color.gray;
        previouslySelectedRarityButton.colors = unselectedColors;
        previouslySelectedRarityText.color = Color.black;

        ColorBlock selectedColors = selectedRarityButton.colors;
        selectedColors.normalColor = Color.black;
        selectedColors.highlightedColor = Color.black;
        selectedRarityButton.colors = selectedColors;
        selectedRarityText.color = Color.white;

        RepopulateItems();

        craftDialog.SetActive( false );
        scavengeOnlyDialog.SetActive( false );
    }

    void RepopulateItems()
    {
        foreach( GameObject itemScreen in itemScreens )
        {
            GameObject.Destroy( itemScreen );
        }
        itemScreens.Clear();

        storedItemsOfCategoryAndRarity.Clear();
        storedItemsOfCategoryAndRarity = storageManager.GetStoredItems( currentCategory, currentRarity );

        foreach( Item_And_Quantity storedItem in storedItemsOfCategoryAndRarity )
        {
            if( storedItem.item.requiredMaterialsToCraft != null && storedItem.item.requiredMaterialsToCraft.requiredItems != null && storedItem.item.requiredMaterialsToCraft.requiredItems.Count > 0 )
            {
                GameObject newItemScreen = GameObject.Instantiate( itemTemplate );
                newItemScreen.transform.SetParent( itemPanelParent, false );
                Button button = newItemScreen.AddComponent<Button>();
                button.onClick.AddListener( () => OnClickCraftableItem( storedItem.item ) );
                itemScreens.Add( newItemScreen );

                Screen_Item_Manager itemScreen = newItemScreen.GetComponent<Screen_Item_Manager>();
                itemScreen.nameText.text = storedItem.item.name;
                itemScreen.quantityText.text = storedItem.quantity.ToString();
                itemScreen.image.sprite = storedItem.item.storageSprite;
            }
            else
            {
                GameObject newItemScreen = GameObject.Instantiate( itemTemplate );
                newItemScreen.transform.SetParent( itemPanelParent, false );
                Button button = newItemScreen.AddComponent<Button>();
                button.onClick.AddListener( () => OnClickScavengableOnlyItem( storedItem.item ) );
                itemScreens.Add( newItemScreen );

                Screen_Item_Manager itemScreen = newItemScreen.GetComponent<Screen_Item_Manager>();
                itemScreen.nameText.text = storedItem.item.name;
                itemScreen.quantityText.text = storedItem.quantity.ToString();
                itemScreen.image.sprite = storedItem.item.storageSprite;
            }
        }
    }

    void OnClickScavengableOnlyItem( Item scavengableItem )
    {
        scavengeOnlyDialog.SetActive( true );
    }

    void OnClickCraftableItem( Item craftableItem )
    {
        selectedItemToCraft = craftableItem;

        foreach( GameObject craftItemScreen in requiredCraftItemsScreens )
        {
            GameObject.Destroy( craftItemScreen );
        }
        requiredCraftItemsScreens.Clear();

        craftDialog.SetActive( true );

        bool canCraft = true;
        foreach( Item_And_Quantity requiredItem in selectedItemToCraft.requiredMaterialsToCraft.requiredItems )
        {
            Item_And_Quantity storedRequiredItem = storageManager.GetStoredItemAndQuantity( requiredItem );

            GameObject newCraftItemScreen = GameObject.Instantiate( itemTemplate );
            newCraftItemScreen.transform.SetParent( craftMaterialRequirementsParent, false );
            requiredCraftItemsScreens.Add( newCraftItemScreen );

            Screen_Item_Manager newCraftItemManager = newCraftItemScreen.GetComponent<Screen_Item_Manager>();
            newCraftItemManager.nameText.text = requiredItem.item.name;
            newCraftItemManager.quantityText.text = storedRequiredItem.quantity + "/" + requiredItem.quantity.ToString();
            newCraftItemManager.image.sprite = requiredItem.item.storageSprite;

            if( storedRequiredItem.quantity >= requiredItem.quantity )
            {
                newCraftItemManager.quantityText.color = Color.green;
            }
            else
            {
                newCraftItemManager.quantityText.color = Color.red;
                canCraft = false;
            }
        }

        if( canCraft )
        {
            craftButton.interactable = true;
        }
        else
        {
            craftButton.interactable = false;
        }
    }

    void OnClickCraft()
    {
        Item_And_Quantity craftedItem = new Item_And_Quantity( selectedItemToCraft, 1 );
        storageManager.AddQuantity( craftedItem );

        foreach( Item_And_Quantity requiredItem in selectedItemToCraft.requiredMaterialsToCraft.requiredItems )
        {
            storageManager.SubtractQuantity( requiredItem );
        }

        RepopulateItems();
        OnClickCraftableItem( selectedItemToCraft );
    }

    public void ShowScreen()
    {
        gameObject.SetActive( true );

        craftDialog.SetActive( false );
        scavengeOnlyDialog.SetActive( false );

        OnClickCategoryButton( 0 );
        OnClickRarityButton( 0 );
    }

    public void HideScreen()
    {
        gameObject.SetActive( false );
    }

    void OnBackButtonClicked()
    {
        gameStateManager.ChangeState( GameState.State.HOME );
    }
}
