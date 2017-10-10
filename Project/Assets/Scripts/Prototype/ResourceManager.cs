// RATIONALE ------------------------------------
// * Manages the GUI related to resources
// * Manages spawning resources

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour 
{
    // GUI --------------------------------------
    public GameObject resourcesGUI;

    // References -------------------------------
    public GameStateManager gameStateManager;
    public TimeManager timeManager;

    // Water & Food -----------------------------
    public FoodManager foodManager;
    public WaterManager waterManager;

    // Delays
    public uint delayBeforeGatherWaterDays = 0;

    [Range(0,24)]
    public uint delayBeforeGatherWaterHours = 0;

    [Range(0,60)]
    public uint delayBeforeGatherWaterMinutes = 60;

    public uint delayBeforeGatherFoodDays = 0;

    [Range(0,24)]
    public uint delayBeforeGatherFoodHours = 0;

    [Range(0,60)]
    public uint delayBeforeGatherFoodMinutes = 60;

    private bool canGatherWater = false;
    private bool canGatherFood = false;

    private TimeManager.Timestamp timeWhenCanGatherWater;
    private TimeManager.Timestamp timeWhenCanGatherFood;

    public Button gatherWaterButton;
    public Button gatherFoodButton;

	// Unity callbacks -----------------------------
	void Start () 
    {
        // GUI
		resourcesGUI.SetActive( false );

        gatherWaterButton.gameObject.SetActive( false );
        gatherFoodButton.gameObject.SetActive( false );

        gatherWaterButton.onClick.AddListener( OnGatheredWaterClicked );
        gatherFoodButton.onClick.AddListener( OnGatheredFoodClicked );

        // GameState
        gameStateManager.RegisterOnGameStateChanged( OnGameStateChanged );  
	}
	
	void Update ()
    {
        if( gameStateManager.GetState() == GameState.State.DAY )
        {
            // Water
            if( !canGatherWater && ReadyToSpawnWater() )
            {
                SpawnWater();
            }

            // Food
            if( !canGatherFood && ReadyToSpawnFood() )
            {
                SpawnFood();
            }
        }
	}

    // Water & Food -----------------------------
    bool ReadyToSpawnWater()
    {
        TimeManager.Timestamp currentTime = timeManager.GetTime();
        
        if( currentTime.day > timeWhenCanGatherWater.day )
        {
            return true;
        }
        else if( currentTime.day == timeWhenCanGatherWater.day && currentTime.hour > timeWhenCanGatherWater.hour )
        {
            return true;
        }
        else if( currentTime.day == timeWhenCanGatherWater.day && currentTime.hour == timeWhenCanGatherWater.hour && currentTime.minute >= timeWhenCanGatherWater.minute )
        {
            return true;
        }

        return false;
    }

    bool ReadyToSpawnFood()
    {
        TimeManager.Timestamp currentTime = timeManager.GetTime();
        
        if( currentTime.day > timeWhenCanGatherFood.day )
        {
            return true;
        }
        else if( currentTime.day == timeWhenCanGatherFood.day && currentTime.hour > timeWhenCanGatherFood.hour )
        {
            return true;
        }
        else if( currentTime.day == timeWhenCanGatherFood.day && currentTime.hour == timeWhenCanGatherFood.hour && currentTime.minute > timeWhenCanGatherFood.minute )
        {
            return true;
        }

        return false;
    }

    void SetWhenCanGatherWater() 
    {
        timeWhenCanGatherWater = timeManager.GetTime();
        timeWhenCanGatherWater.day += delayBeforeGatherWaterDays;
        timeWhenCanGatherWater.hour += delayBeforeGatherWaterHours;
        timeWhenCanGatherWater.minute += delayBeforeGatherWaterMinutes;
    }

    void SetWhenCanGatherFood() 
    {
        timeWhenCanGatherFood = timeManager.GetTime();
        timeWhenCanGatherFood.day += delayBeforeGatherFoodDays;
        timeWhenCanGatherFood.hour += delayBeforeGatherFoodHours;
        timeWhenCanGatherFood.minute += delayBeforeGatherFoodMinutes;
    }

    void SpawnWater()
    {
        canGatherWater = true;
        gatherWaterButton.gameObject.SetActive( true );
    }

    void SpawnFood()
    {
        canGatherFood = true;
        gatherFoodButton.gameObject.SetActive( true );
    }

    private void DisableGatherWater()
    {
        canGatherWater = false;
        gatherWaterButton.gameObject.SetActive( false );
    }

    private void DisableGatherFood()
    {
        canGatherFood = false;
        gatherFoodButton.gameObject.SetActive( false );
    }

    void OnGatheredWaterClicked()
    {
        canGatherWater = false;
        gatherWaterButton.gameObject.SetActive( false );
        waterManager.OnGathered();
        SetWhenCanGatherWater();
    }

    void OnGatheredFoodClicked()
    {
        canGatherFood = false;
        gatherFoodButton.gameObject.SetActive( false );
        foodManager.OnGathered();
        SetWhenCanGatherFood();
    }

    // GameState --------------------------------
    void OnGameStateChanged( GameState.State newState, GameState.State previousState )
    {
        if( previousState == GameState.State.CHARACTER_SELECT )
        {
            SetWhenCanGatherWater();
            SetWhenCanGatherFood();
        }

        switch( newState )
        {
            case GameState.State.STATUS:
                DisableGatherFood();
                DisableGatherWater();
                resourcesGUI.SetActive( true );
                break;
            case GameState.State.DAY:
                resourcesGUI.SetActive( true );
                break;
            default:
                resourcesGUI.SetActive( false );
                break;
        }
    }
}
