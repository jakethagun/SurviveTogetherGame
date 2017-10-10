using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Rationale:
// * Manages the current character and related traits
// * Manages the character related GUI

public class CharacterManager : MonoBehaviour
{
    // References -------------------------------
    public GameStateManager gameStateManager;

    // GUI --------------------------------------
    public GameObject characterSelectionGUI;
    public Text characterNameGUI;

    public Button selectSarah;
    public Button selectJohn;
    public Button selectJeremy;
    public Button selectJessica;
    public Button selectSam;

    // Character --------------------------------
    public GameObject[] characterPrefabs;
    private int selectedCharacter = 0;
    private GameObject characterGO;
    private Character character;

    void CreateCharacter()
    {
        characterGO = GameObject.Instantiate( characterPrefabs[ selectedCharacter ] );
        character = characterGO.GetComponent< Character >();
    }

    // Unity Callbacks --------------------------
	void Start ()
    {
        // Gui
        characterNameGUI.enabled = false;

        selectSarah.onClick.AddListener( OnSarahSelected );
        selectJohn.onClick.AddListener( OnJohnSelected );
        selectJeremy.onClick.AddListener( OnJeremySelected );
        selectJessica.onClick.AddListener( OnJessicaSelected );
        selectSam.onClick.AddListener( OnSamSelected );

        // GameState
        gameStateManager.RegisterOnGameStateChanged( OnGameStateChanged );
    }
	
	void Update ()
    {
	}

    // Character --------------------------------
    public Character GetCharacter()
    {
        return character;
    }

    // GameState --------------------------------
    void OnGameStateChanged( GameState.State newState, GameState.State previousState )
    {
        if( previousState == GameState.State.CHARACTER_SELECT )
        {
            CreateCharacter();
            characterNameGUI.text = character.name;

            // Turn off selection GUI
            characterSelectionGUI.SetActive( false );
        }

        switch( newState )
        {
            case GameState.State.DAY:
            case GameState.State.STATUS:
                ActivateCharacterName();
                break;
            default:
                DeactivateCharacterName();
                break;
        }
    }

    // GUI --------------------------------------
    void ActivateCharacterName()
    {
        characterNameGUI.enabled = true;
    }

    void DeactivateCharacterName()
    {
        characterNameGUI.enabled = false;
    }

    // Selection Buttons ------------------------
    void OnSarahSelected()
    {
        selectedCharacter = 0;
        gameStateManager.TransitionToNextState();
    }

    void OnJohnSelected()
    {
        selectedCharacter = 1;
        gameStateManager.TransitionToNextState();
    }

    void OnJeremySelected()
    {
        selectedCharacter = 2;
        gameStateManager.TransitionToNextState();
    }

    void OnJessicaSelected()
    {
        selectedCharacter = 3;
        gameStateManager.TransitionToNextState();
    }

    void OnSamSelected()
    {
        selectedCharacter = 4;
        gameStateManager.TransitionToNextState();
    }
}
