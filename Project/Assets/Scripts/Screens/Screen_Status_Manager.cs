using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Status_Manager : MonoBehaviour {

    // References
    public Button backButton;

    GameState_Manager gameStateManager;
    Character_Manager characterManager;
    Survivor_Manager survivorManager;

    public GameObject templateCharacterStatus;
    public GameObject characterStatusPanel;
    
    List< GameObject > statusPanels = new List< GameObject >();

	// Use this for initialization
	void Awake ()
    {
        gameStateManager = GameObject.Find( "GameState_Manager" ).GetComponent< GameState_Manager >();
        characterManager = GameObject.Find( "Character_Manager" ).GetComponent< Character_Manager >();
        survivorManager = GameObject.Find( "Survivor_Manager" ).GetComponent< Survivor_Manager >();    
        
        backButton.onClick.AddListener( OnBackClicked );
	}

    void PopulateCharacterStatusInstances()
    {
        uint charactersAndSurvivors = 1 + survivorManager.NumberOfSurvivors();

        // Spawn
        foreach( GameObject panel in statusPanels )
        {
            GameObject.Destroy( panel );
        }
        statusPanels.Clear();

        for( uint i = 0; i < charactersAndSurvivors; ++i )
        {
            GameObject statusPanel = GameObject.Instantiate( templateCharacterStatus, characterStatusPanel.transform, false );
            statusPanels.Add( statusPanel );
        }
    }

    void DelegateCharactersAndSurvivors()
    {
        // Character
        {
            Character character = characterManager.GetCharacter();
            Status_Panel_Manager panelManager = statusPanels[ 0 ].GetComponent< Status_Panel_Manager >();
            panelManager.TargetCharacter( character );
        }

        // Survivors
        foreach( GameObject statusPanel in statusPanels )
        {
            Status_Panel_Manager panelManager = statusPanel.GetComponent< Status_Panel_Manager >();

            if( !panelManager.HasTarget() )
            {
                
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // GUI
    void OnBackClicked()
    {
        gameStateManager.ChangeState( GameState.State.HOME );
    }

    public void ShowScreen()
    {
        gameObject.SetActive( true );
        PopulateCharacterStatusInstances();
        DelegateCharactersAndSurvivors();
    }

    public void HideScreen()
    {
        gameObject.SetActive( false );
    }
}
