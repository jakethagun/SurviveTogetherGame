using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Storage_Manager : MonoBehaviour {

    public Transform itemPanelParent;
    public GameObject itemTemplate;
    public Button backButton;

    public Button[] categoryButtons;
    public Text[] categoryButtonsText;

    public Button[] rarityButtons;
    public Text[] rarityButtonsText;

    List<GameObject> itemScreens = new List<GameObject>();

    private GameState_Manager gameStateManager;
    Storage_Manager storageManager;

    Item.Category currentCategory = Item.Category.HOME;
    Item.Rarity currentRarity = Item.Rarity.COMMON;

    Button selectedCategoryButton;
    Text selectedCategoryText;

    Button selectedRarityButton;
    Text selectedRarityText;

    List<Item_And_Quantity> storedItemsOfCategoryAndRarity = new List<Item_And_Quantity>();

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
    }

    void OnClickCategoryButton( int index )
    {
        currentCategory = (Item.Category)index;

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
            if( storedItem.quantity > 0 )
            {
                GameObject newItemScreen = GameObject.Instantiate( itemTemplate );
                newItemScreen.transform.SetParent( itemPanelParent, false );
                itemScreens.Add( newItemScreen );

                Screen_Item_Manager itemScreen = newItemScreen.GetComponent<Screen_Item_Manager>();
                itemScreen.nameText.text = storedItem.item.name;
                itemScreen.quantityText.text = storedItem.quantity.ToString();
                itemScreen.image.sprite = storedItem.item.storageSprite;
            }
        }
    }

    public void ShowScreen()
    {
        gameObject.SetActive( true );

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
