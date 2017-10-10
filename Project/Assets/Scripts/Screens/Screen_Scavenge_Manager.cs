using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Scavenge_Manager : MonoBehaviour
{
    public GameObject taskManagerTemplate;
    public Transform taskManagerPanelParent;

    public List<Button> locationButtons;

    List<GameObject> taskPanels = new List<GameObject>();

    public Button arrowButton;
    public Button scavengeButton;

    public GameObject locationInformation;
    public Text locationNameText;
    public Text locationDescriptionText;
    public Text locationItemsText;

    GameState_Manager gameStateManager;
    Character_Manager characterManager;
    Scavenging_Manager scavengingManager;
    
    Scavenging_Location selectedLocation;
    Scavenging_Locations_Manager locationsManager;

    int currentMap = 0;

    void Awake()
    {
        gameStateManager = GameObject.Find( "GameState_Manager" ).GetComponent<GameState_Manager>();
        characterManager = GameObject.Find( "Character_Manager" ).GetComponent<Character_Manager>();
        scavengingManager = GameObject.Find( "Scavenging_Manager" ).GetComponent<Scavenging_Manager>();
        
        locationsManager = GameObject.Find( "Scavenging_Locations_Manager" ).GetComponent<Scavenging_Locations_Manager>();
        arrowButton.onClick.AddListener( OnClickNextMap );
        scavengeButton.onClick.AddListener( OnClickScavenge );

        selectedLocation = null;
    }

    void Update()
    {
        bool allHaveTasks = true;
        foreach( GameObject taskPanel in taskPanels )
        {
            if( !taskPanel.GetComponent<Screen_Scavenge_Task_Manager>().HasTask() )
            {
                allHaveTasks = false;
                break;
            }
        }

        if( allHaveTasks && selectedLocation != null )
        {
            scavengeButton.interactable = true;
        }
        else
        {
            scavengeButton.interactable = false;
        }
    }

    public void ShowScreen()
    {
        gameObject.SetActive( true );

        selectedLocation = null;

        foreach( GameObject taskPanel in taskPanels )
        {
            GameObject.Destroy( taskPanel );
        }
        taskPanels.Clear();

        // Character
        Character character = characterManager.GetCharacter();

        GameObject characterTaskPanel = GameObject.Instantiate( taskManagerTemplate, taskManagerPanelParent, false );
        Screen_Scavenge_Task_Manager taskManager = characterTaskPanel.GetComponent<Screen_Scavenge_Task_Manager>();
        taskManager.TargetCharacter( character );
        taskPanels.Add( characterTaskPanel );

        currentMap = 0;
        RepopulateLocations();
    }

    public void RepopulateLocations()
    {
        foreach( Button locationButton in locationButtons )
        {
            locationButton.onClick.RemoveAllListeners();
            locationButton.gameObject.SetActive( false );
        }

        // Set locations
        List<Scavenging_Location> locations = locationsManager.locations;

        int offset = currentMap * locationButtons.Count;
        for( int index = 0; index + offset < locations.Count && index < locationButtons.Count; ++index )
        {
            Scavenging_Location location = locations[index + offset];
            locationButtons[index].gameObject.SetActive( true );
            locationButtons[index].image.sprite = location.sprite;
            int currentIndex = index;
            locationButtons[index].onClick.AddListener( () => OnClickLocation( location ) );
        }
    }

    void OnClickNextMap()
    {
        ++currentMap;
        RepopulateLocations();
    }

    void OnClickLocation( Scavenging_Location location )
    {
        selectedLocation = location;

        locationInformation.SetActive( true );

        locationNameText.text = location.name;
        locationDescriptionText.text = location.description;

        locationItemsText.text = "";
        foreach( Scavengable_Item item in location.ScavengableItems() )
        {
            locationItemsText.text += item.scavengableItem.name + '\n';
        }        
    }

    void OnClickScavenge()
    {
        scavengingManager.Scavenge( selectedLocation );
        gameStateManager.ChangeState( GameState.State.SCAVENGED );
    }

    public void HideScreen()
    {
        gameObject.SetActive( false );
    }
}
