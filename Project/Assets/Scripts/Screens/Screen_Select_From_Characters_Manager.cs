using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Select_From_Characters_Manager : MonoBehaviour 
{
    // Template
    public GameObject characterButtonPanel;
    public GameObject templateButtonCharacterSelect;

    // References
    public Default_Characters_Manager defaultCharactersManager;

    GameState_Manager gameStateManager;

    string characterSelectedName = "";

	// Unity callbacks
	void Start () 
    {
        gameStateManager = GameObject.Find( "GameState_Manager" ).GetComponent< GameState_Manager >();

        PopulateCharacterSelectButtons();  
	}

	void Update ()
    {
		
	}

    // Populating Buttons
    private void PopulateCharacterSelectButtons()
    {
        List< Character > characters = defaultCharactersManager.GetDefaultCharacters();
        
        foreach (var character in characters )
        {
            GameObject characterSelectButton = GameObject.Instantiate( templateButtonCharacterSelect );
            characterSelectButton.name = "Button_Select_" + character.name;
            characterSelectButton.SetActive( true );
            characterSelectButton.transform.SetParent( characterButtonPanel.transform );
            characterSelectButton.GetComponent< Image >().sprite = character.selectFromCharactersSprite;
            characterSelectButton.GetComponent< Button >().onClick.AddListener( () => CharacterSelected( character.name ) );
            characterSelectButton.transform.localScale = new Vector3( 1.0f, 1.0f );
        }
    }

    public void ShowScreen()
    {
        gameObject.SetActive( true );
    }

    public void HideScreen()
    {
        gameObject.SetActive( false );
    }

    void CharacterSelected( string characterName )
    {
        characterSelectedName = characterName;

        gameStateManager.ChangeState( GameState.State.SELECT_CHARACTER );
    }

    public string GetCharacterSelected()
    {
        return characterSelectedName;
    }
}
