using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Select_Character_Manager : MonoBehaviour 
{
    // GUI
    public Text characterNameText;
    public Text characterBioText;
    public Image characterImage;

    public Button trait0Button;
    public Button trait1Button;
    public Button trait2Button;

    public Button backButton;
    public Button selectButton;

    public Transform traitPopupParent;

    public GameObject confirmCharacterSelectTemplate;
    GameObject confirmScreenInstance;

    // References
    Default_Characters_Manager defaultCharactersManager;
    GameState_Manager gameStateManager;
    Traits_Manager traitsManager;

    void Awake()
    {
        defaultCharactersManager = GameObject.Find( "Default_Characters_Manager" ).GetComponent< Default_Characters_Manager >();
        gameStateManager = GameObject.Find( "GameState_Manager" ).GetComponent< GameState_Manager >();
        traitsManager = GameObject.Find( "Traits_Manager" ).GetComponent<Traits_Manager>();
    }

	// Use this for initialization
	void Start ()
    {
		backButton.onClick.AddListener( OnClickedBack );
        selectButton.onClick.AddListener( OnClickedSelect );
	}

	// Update is called once per frame
	void Update ()
    {
		
	}

    public void ShowScreen( string characterName )
    {
        gameObject.SetActive( true );
        characterNameText.text = characterName;
        Character character = defaultCharactersManager.GetDefaultCharacter( characterName );

        characterBioText.text = character.bio;
        characterImage.sprite = character.selectCharacterSprite;

        Character.Trait trait0 = character.traitsAndMagnitudes[ 0 ].trait;
        Character.Trait trait1 = character.traitsAndMagnitudes[ 1 ].trait;
        Character.Trait trait2 = character.traitsAndMagnitudes[ 2 ].trait;

        trait0Button.image.sprite = traitsManager.traitsSprites[ (int)trait0 ];
        trait1Button.image.sprite = traitsManager.traitsSprites[ (int)trait1 ];
        trait2Button.image.sprite = traitsManager.traitsSprites[ (int)trait2 ];

        trait0Button.onClick.AddListener( () => TraitButtonClicked( trait0 ) );
        trait1Button.onClick.AddListener( () => TraitButtonClicked( trait1 ) );
        trait2Button.onClick.AddListener( () => TraitButtonClicked( trait2 ) );
    }

    void TraitButtonClicked( Character.Trait trait )
    {
        GameObject spawnedTrait = GameObject.Instantiate( traitsManager.traitsPopupScreens[( int )trait], traitPopupParent, false );
    }

    public void HideScreen()
    {
        gameObject.SetActive( false );

        trait0Button.onClick.RemoveAllListeners();
        trait1Button.onClick.RemoveAllListeners();
        trait2Button.onClick.RemoveAllListeners();
    }

    void OnClickedSelect()
    {
        confirmScreenInstance = GameObject.Instantiate( confirmCharacterSelectTemplate );
        confirmScreenInstance.transform.SetParent( GameObject.Find( "Canvas" ).transform, false );

        Screen_Character_Confirm_Popup_Manager confirmScreenManager = confirmScreenInstance.GetComponent<Screen_Character_Confirm_Popup_Manager>();

        confirmScreenManager.yesButton.onClick.AddListener( OnClickedYes );
        confirmScreenManager.noButton.onClick.AddListener( OnClickedNo );
    }

    void OnClickedYes()
    {
        Character_Manager characterManager = GameObject.Find( "Character_Manager" ).GetComponent<Character_Manager>();
        characterManager.SetCharacter( characterNameText.text );
        gameStateManager.ChangeState( GameState.State.HOME );

        GameObject.Destroy( confirmScreenInstance );
    }

    void OnClickedNo()
    {
        gameStateManager.ChangeState( GameState.State.SELECT_FROM_CHARACTERS );

        GameObject.Destroy( confirmScreenInstance );
    }

    void OnClickedBack()
    {
        gameStateManager.ChangeState( GameState.State.SELECT_FROM_CHARACTERS );
    }
}
