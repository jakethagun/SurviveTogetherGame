using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Scavenged_Manager : MonoBehaviour {

    public Transform panelRewardsParent;
    public GameObject itemTemplate;

    public Button nextButton;

    List<GameObject> shownItems = new List<GameObject>();

    Scavenging_Manager scavengingManager;
    GameState_Manager gamestateManager;

    private void Awake()
    {
        scavengingManager = GameObject.Find( "Scavenging_Manager" ).GetComponent<Scavenging_Manager>();
        gamestateManager = GameObject.Find( "GameState_Manager" ).GetComponent<GameState_Manager>();
        nextButton.onClick.AddListener( OnClickNext );
    }

    public void OnClickNext()
    {
        gamestateManager.ChangeState( GameState.State.HOME );
    }

    public void ShowScreen()
    {
        gameObject.SetActive( true );

        List<Item_And_Quantity> scavengedItems = scavengingManager.PreviousScavengedItems();

        foreach( Item_And_Quantity scavengedItem in scavengedItems )
        {
            GameObject item = GameObject.Instantiate( itemTemplate, panelRewardsParent, false );
            Screen_Item_Manager manager = item.GetComponent<Screen_Item_Manager>();

            manager.nameText.text = scavengedItem.item.name;
            manager.image.sprite = scavengedItem.item.storageSprite;
            manager.quantityText.text = scavengedItem.quantity.ToString();            

            shownItems.Add( item );
        }
    }

    public void HideScreen()
    {
        foreach( GameObject item in shownItems )
        {
            GameObject.Destroy( item );
        }
        shownItems.Clear();

        gameObject.SetActive( false );
    }
}
