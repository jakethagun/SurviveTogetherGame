// RATIONALE ------------------------------------
// * Manages the status GUI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour {

    public uint hoursBelowForSlightlyThirsty = 48;
    public uint hoursBelowForThirsty = 24;
    public uint hoursBelowForVeryThirsty = 12;

    public uint hoursBelowForSlightlyHungry = 48;
    public uint hoursBelowForHungry = 24;
    public uint hoursBelowForVeryHungry = 12;

    public uint hoursGainedByHydrating = 10;
    public uint hoursGainedByFeeding = 10;

    // GUI
    public Button statusButton;
    public Text characterHydratedStatusText;
    public Text characterFedStatusText;
    public Button hydrateButton;
    public Button feedButton;
    public Text sicknessText;
    public Text injuryText;

    // References
    public GameStateManager gameStateManager;
    public CharacterManager characterManager;
    public WaterManager waterManager;
    public FoodManager foodManager;
    public Time_Manager timeManager;

	// Unity Callbacks -----------------------------
	void Start () 
    {
        statusButton.onClick.AddListener( OnClickedStatus );
        hydrateButton.onClick.AddListener( OnClickedHydrate );
        feedButton.onClick.AddListener( OnClickedFeed );

        DisableCharacterStatus();
	    DisableStatusButton();

        // GameState
        gameStateManager.RegisterOnGameStateChanged( OnGameStateChanged );
	}
	
	void Update () 
    {
        if( gameStateManager.GetState() == GameState.State.STATUS )
        {
            Character character = characterManager.GetCharacter();

            if( waterManager.GetWater() >= 1 )
            {
                EnableHydrate();
            }
            else
            {
                DisableHydrate();
            }

            if( foodManager.GetFood() >= 1 )
            {
                EnableFeed();
            }
            else
            {
                DisableFeed();
            }
    
            Timestamp timeWhenDeadToThirst = character.GetTimeWhenDeadToThirst();
            Timestamp timeWhenDeadToHunger = character.GetTimeWhenDeadToHunger();

            Timestamp currentTime = timeManager.GetTime();
            uint hoursUntilDeadFromThirst = timeManager.HoursBetweenTimestamps( currentTime, timeWhenDeadToThirst );
            uint hoursUntilDeadFromHunger = timeManager.HoursBetweenTimestamps( currentTime, timeWhenDeadToHunger );

            if( hoursUntilDeadFromThirst == 0 )
            {
                characterHydratedStatusText.text = "Dead from thirst";
            }
            else if( hoursUntilDeadFromThirst < hoursBelowForVeryThirsty )
            {
                characterHydratedStatusText.text = "Very Thirsty";
            }
            else if( hoursUntilDeadFromThirst < hoursBelowForThirsty )
            {
                characterHydratedStatusText.text = "Thirsty";
            }
            else if( hoursUntilDeadFromThirst < hoursBelowForSlightlyThirsty )
            {
                characterHydratedStatusText.text = "Slightly Thirsty";
            }
            else
            {
                characterHydratedStatusText.text = "Hydrated";
                DisableHydrate();
            }

            if( hoursUntilDeadFromHunger == 0 )
            {
                characterFedStatusText.text = "Dead from hunger";
            }
            else if( hoursUntilDeadFromHunger < hoursBelowForVeryHungry )
            {
                characterFedStatusText.text = "Very Hungry";
            }
            else if( hoursUntilDeadFromHunger < hoursBelowForHungry )
            {
                characterFedStatusText.text = "Hungry";
            }
            else if( hoursUntilDeadFromHunger < hoursBelowForSlightlyHungry )
            {
                characterFedStatusText.text = "Slightly Hungry";
            }
            else
            {
                characterFedStatusText.text = "Well Fed";
                DisableFeed();
            }

            // SICKNESS
            switch( character.GetSickness() )
            {
                case Character.Sickness.HEALTHY:
                    sicknessText.text = "Healthy";
                    break;
                case Character.Sickness.SLIGHTLY_SICK:
                    sicknessText.text = "Slightly Sick";
                    break;
                case Character.Sickness.SICK:
                    sicknessText.text = "Sick";
                    break;
                case Character.Sickness.VERY_SICK:
                    sicknessText.text = "Very Sick";
                    break;
            }

            // INJURY
            switch( character.GetInjury() )
            {
                case Character.Injury.NOT_INJURED:
                    injuryText.text = "Not Injured";
                    break;
                case Character.Injury.SLIGHTLY_INJURED:
                    injuryText.text = "Slightly Injured";
                    break;
                case Character.Injury.INJURED:
                    injuryText.text = "Injured";
                    break;
                case Character.Injury.VERY_INJURED:
                    injuryText.text = "Very Injured";
                    break;
            }
        }
	}

    // GUI --------------------------------------
    private void EnableStatusButton()
    {
        statusButton.gameObject.SetActive( true );
    }

    private void DisableStatusButton()
    {
        statusButton.gameObject.SetActive( false );
    }

    private void EnableCharacterStatus()
    {
        characterHydratedStatusText.gameObject.SetActive( true );
        characterFedStatusText.gameObject.SetActive( true );
        injuryText.gameObject.SetActive( true );
        sicknessText.gameObject.SetActive( true );
    }

    private void DisableCharacterStatus()
    {
        characterHydratedStatusText.gameObject.SetActive( false );
        characterFedStatusText.gameObject.SetActive( false );
        injuryText.gameObject.SetActive( false );
        sicknessText.gameObject.SetActive( false );
    }

    private void EnableHydrate()
    {
        hydrateButton.gameObject.SetActive( true );
    }

    private void DisableHydrate()
    {
        hydrateButton.gameObject.SetActive( false );
    }

    private void EnableFeed()
    {
        feedButton.gameObject.SetActive( true );
    }

    private void DisableFeed()
    {
        feedButton.gameObject.SetActive( false );
    }

    private void OnClickedStatus() {
        if( gameStateManager.GetState() != GameState.State.STATUS )
        {
            gameStateManager.ChangeState( GameState.State.STATUS );
        }
        else
        {
            gameStateManager.ChangeState( GameState.State.DAY );
        }
    }

    private void OnClickedHydrate() {
        Character character = characterManager.GetCharacter();
        if( waterManager.GetWater() >= 1 )
        {
            waterManager.ConsumeWater( 1 );
            character.Hydrate( hoursGainedByHydrating );
        }
    }

    private void OnClickedFeed() {
        Character character = characterManager.GetCharacter();
        
        if( foodManager.GetFood() >= 1 )
        {
            foodManager.ConsumeFood( 1 );
            character.Feed( hoursGainedByFeeding );
        }
    }

    // GameState --------------------------------
    void OnGameStateChanged(GameState.State newState, GameState.State previousState)
    {
        switch( newState )
        {
            case GameState.State.STATUS:
                EnableCharacterStatus();
                break;
            case GameState.State.DAY:
                EnableStatusButton();
                DisableCharacterStatus();
                DisableHydrate();
                DisableFeed();
                break;
            case GameState.State.END_OF_DAY:
            default:
                DisableCharacterStatus();
                DisableHydrate();
                DisableFeed();
                DisableStatusButton();
                break;
        }
    }
}
