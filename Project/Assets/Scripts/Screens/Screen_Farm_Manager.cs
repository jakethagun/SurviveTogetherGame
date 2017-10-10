using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Farm_Manager : MonoBehaviour
{
    public Text rateText;
    public Text totalText;

    public Image foregroundImage;

    public GameObject itemTemplate;
    public Transform itemPanelParent;
    List<GameObject> itemScreens = new List<GameObject>();
    List<Item_And_Quantity> storedFood = new List<Item_And_Quantity>();

    public Button backButton;

    GameState_Manager gameStateManager;
    Storage_Manager storageManager;

    Farm_Manager farmManager;
    Color defaultForegroundColor;

    void Awake()
    {
        storageManager = GameObject.Find( "Storage_Manager" ).GetComponent<Storage_Manager>();
        farmManager = GameObject.Find( "Farm_Manager" ).GetComponent<Farm_Manager>();
        gameStateManager = GameObject.Find( "GameState_Manager" ).GetComponent<GameState_Manager>();
        backButton.onClick.AddListener( OnBackButtonClicked );

        defaultForegroundColor = foregroundImage.color;
    }

    void OnBackButtonClicked()
    {
        gameStateManager.ChangeState( GameState.State.HOME );
    }

    void RepopulateItems()
    {
        foreach( GameObject itemScreen in itemScreens )
        {
            GameObject.Destroy( itemScreen );
        }
        itemScreens.Clear();

        storedFood.Clear();
        storedFood = storageManager.GetStoredItems( Item.Category.FOOD );

        foreach( Item_And_Quantity storedItem in storedFood )
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

    void Refresh()
    {
        uint currentFarmFood = farmManager.CurrentFarmFood();
        float farmSystemQuantity = farmManager.GetFarmSystemQuantity();
        float farmSystemRate = farmManager.GetFarmSystemRatePerHour();
        float farmSystemMaximum = farmManager.GetMaximumFarmSystemQuantity();

        totalText.text = ( currentFarmFood ).ToString();

        rateText.text = "Rate : " + ( ( int )farmSystemRate ).ToString();

        foregroundImage.fillAmount = farmSystemQuantity / farmSystemMaximum;

        if( farmSystemQuantity / farmSystemMaximum > 0.99f )
        {
            if( foregroundImage.gameObject.GetComponent<Button>() == null )
            {
                Button harvest = foregroundImage.gameObject.AddComponent<Button>();
                harvest.onClick.AddListener( OnClickedHarvestFarm );
                foregroundImage.color = Color.green;
            }
        }
        else
        {
            foregroundImage.color = defaultForegroundColor;
        }
    }

    void OnClickedHarvestFarm()
    {
        farmManager.HarvestFarm();
        Destroy( foregroundImage.gameObject.GetComponent<Button>() );
        RepopulateItems();
    }

    void Update()
    {
        Refresh();
    }

    public void ShowScreen()
    {
        gameObject.SetActive( true );
        RepopulateItems();
        Refresh();
    }

    public void HideScreen()
    {
        gameObject.SetActive( false );
    }
}
