using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Upgrade_Traits_Manager : MonoBehaviour {

    // References
    public Text traitsPointText;

    GameState_Manager gameStateManager;
    Traits_Manager traitsManager;
    Character_Manager characterManager;
    Survivor_Manager survivorManager;

    List< GameObject > traitsPanels = new List< GameObject >();

    public GameObject templateCharacterStatus;
    public GameObject characterTraitsPanel;

	// Use this for initialization
	void Awake ()
    {
		traitsManager = GameObject.Find( "Traits_Manager" ).GetComponent< Traits_Manager >();
        gameStateManager = GameObject.Find( "GameState_Manager" ).GetComponent< GameState_Manager >();
        characterManager = GameObject.Find( "Character_Manager" ).GetComponent< Character_Manager >();
        survivorManager = GameObject.Find( "Survivor_Manager" ).GetComponent< Survivor_Manager >();
    }

    void Update()
    {
        SetTraitsPointsText();
    }
	
	void PopulateCharacterTraitsInstances()
    {
        uint charactersAndSurvivors = 1 + survivorManager.NumberOfSurvivors();

        // Spawn
        foreach( GameObject panel in traitsPanels )
        {
            GameObject.Destroy( panel );
        }
        traitsPanels.Clear();

        for( uint i = 0; i < charactersAndSurvivors; ++i )
        {
            GameObject statusPanel = GameObject.Instantiate( templateCharacterStatus, characterTraitsPanel.transform, false );
            traitsPanels.Add( statusPanel );
        }

#if false
        // Height
        float maxHeightOfPanels = characterTraitsPanel.GetComponent< RectTransform >().sizeDelta.y - characterTraitsPanel.GetComponent< VerticalLayoutGroup >().padding.top - characterTraitsPanel.GetComponent< VerticalLayoutGroup >().padding.bottom;
        float heightOfPanel = templateCharacterStatus.GetComponent< RectTransform >().sizeDelta.y;
        float heightOfAllPanels = charactersAndSurvivors * heightOfPanel;

        if( heightOfAllPanels > maxHeightOfPanels )
        {
            characterTraitsPanel.GetComponent< VerticalLayoutGroup >().childControlHeight = true;
        }
        else
        {
            characterTraitsPanel.GetComponent< VerticalLayoutGroup >().childControlHeight = false;
        }
#endif
    }

    void DelegateCharactersAndSurvivors()
    {
        // Character
        {
            Character character = characterManager.GetCharacter();
            Traits_Panel_Manager panelManager = traitsPanels[ 0 ].GetComponent< Traits_Panel_Manager >();
            panelManager.TargetCharacter( character );
        }

        // Survivors
        foreach( GameObject statusPanel in traitsPanels )
        {
            Traits_Panel_Manager panelManager = statusPanel.GetComponent< Traits_Panel_Manager >();

            if( !panelManager.HasTarget() )
            {
                
            }
        }
    }

    void SetTraitsPointsText()
    {
        traitsPointText.text = "Traits points : " + traitsManager.GetTraitPoints();
    }

    public void ShowScreen()
    {
        gameObject.SetActive( true );
        SetTraitsPointsText();
        PopulateCharacterTraitsInstances();
        DelegateCharactersAndSurvivors();        
    }

    public void HideScreen()
    {
        gameObject.SetActive( false );
    }
}
