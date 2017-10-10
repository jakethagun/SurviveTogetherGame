using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestManager : MonoBehaviour {

    public uint minimumMaterialsLost = 8;
    public uint maximumMaterialsLost = 10;

    public uint minimumSurvivorsDieing = 3;
    public uint maximumSurvivorsDieing = 5;

    // GUI
    public Button restButton;

    // References
    public GameStateManager gameStateManager;
    public TimeManager timeManager;
    public DefendingManager defendingManager;

	// Unity Callbacks -----------------------------
	void Start ()
    {
        DisableRestButton();

        restButton.onClick.AddListener( OnRestButtonClicked );

	    // GameState
        gameStateManager.RegisterOnGameStateChanged( OnGameStateChanged );	
	}

	void Update ()
    {
	}

    void DisableRestButton()
    {
        restButton.gameObject.SetActive( false );
    }

    void EnableRestButton()
    {
        restButton.gameObject.SetActive( true );
    }

    void OnRestButtonClicked()
    {
        GameState.State state = gameStateManager.GetState();
        
        if( state != GameState.State.RESTING )
        {
            gameStateManager.ChangeState( GameState.State.RESTING );

            DefendingManager.RaidResult result = defendingManager.Raid( minimumMaterialsLost, maximumMaterialsLost, 0, 0, minimumSurvivorsDieing, maximumSurvivorsDieing );
            result.PrintResult();        

            gameStateManager.ChangeState( GameState.State.END_OF_DAY );
        }
    }

    // GameState --------------------------------
    void OnGameStateChanged(GameState.State newState, GameState.State previousState)
    {
        switch( newState )
        {
            case GameState.State.NIGHT:
                EnableRestButton();
                break;
            default:
                DisableRestButton();
                break;
        }
    }
}
