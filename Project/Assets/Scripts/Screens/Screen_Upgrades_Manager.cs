using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Upgrades_Manager : MonoBehaviour {

    public Button backButton;
    public Button upgradeTraitsButton;
    public Button upgradeHomeButton;

    public Text traitsText;
    public Text homeText;

    Button selectedButton = null;
    Text selectedText = null;

    private GameState_Manager gameStateManager;

    void Awake()
    {
        
    }

	// Use this for initialization
	void Start ()
    {
        upgradeHomeButton.onClick.AddListener( OnUpgradeHomeButtonClicked );
        upgradeTraitsButton.onClick.AddListener( OnUpgradeTraitsButtonClicked );

        gameStateManager = GameObject.Find( "GameState_Manager" ).GetComponent<GameState_Manager>();
        gameStateManager.RegisterOnGameStateChanged( OnGameStateChanged );
        OnGameStateChanged( gameStateManager.GetState(), gameStateManager.GetState() );

        backButton.onClick.AddListener( OnBackButtonClicked );
    }

    void OnBackButtonClicked()
    {
        gameStateManager.ChangeState( GameState.State.HOME );
    }

    void OnGameStateChanged( GameState.State newState, GameState.State previousState )
    {
        if( selectedButton && selectedText )
        {
            ColorBlock colors = selectedButton.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = Color.gray;
            selectedButton.colors = colors;

            selectedText.color = Color.black;
        }

        switch( newState )
        {
            case GameState.State.UPGRADE_HOME:
                selectedButton = upgradeHomeButton;
                selectedText = homeText;
                break;
            case GameState.State.UPGRADE_TRAITS:
                selectedButton = upgradeTraitsButton;
                selectedText = traitsText;
                break;
            default:
                selectedButton = null;
                selectedText = null;
                break;
        }

        if( selectedButton && selectedText )
        {
            ColorBlock colors = selectedButton.colors;
            colors.normalColor = Color.black;
            colors.highlightedColor = Color.black;
            selectedButton.colors = colors;

            selectedText.color = Color.white;
        }
    }

    void OnUpgradeHomeButtonClicked()
    {
        gameStateManager.ChangeState( GameState.State.UPGRADE_HOME );
    }

    void OnUpgradeTraitsButtonClicked()
    {
        gameStateManager.ChangeState( GameState.State.UPGRADE_TRAITS );
    }

    public void ShowScreen()
    {
        gameObject.SetActive( true );
    }

    public void HideScreen()
    {
        gameObject.SetActive( false );
    }
}
