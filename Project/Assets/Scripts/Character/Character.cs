using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    // Enums ------------------------------------
    public enum Sex
    {
        MALE,
        FEMALE
    }

    public enum Hunger
    {
        WELL_FED = 0,
        SLIGHTLY_HUNGRY,
        HUNGRY,
        VERY_HUNGRY
    }

    public enum Hydration
    {
        HYDRATED = 0,
        SLIGHTLY_THIRSTY,
        THIRSTY,
        VERY_THIRSTY
    }

    public enum Sickness
    {
        HEALTHY = 0,
        SLIGHTLY_SICK,
        SICK,
        VERY_SICK
    }

    public enum Injury
    {
       NOT_INJURED = 0,
       SLIGHTLY_INJURED,
       INJURED,
       VERY_INJURED
    }

    public enum Trait
    {
        STRENGTH,
        SCAVENGING,
        MEDICAL,
        LUCK,
        NEGOTIATION
    }

    [System.Serializable]
    public class Trait_And_Magnitude
    {
        public Trait trait;
        public uint magnitude;
    }
    

    // References -------------------------------
    GameState_Manager gameStateManager;
    Time_Manager timeManager;

    // Data -------------------------------------
    public int age;
    public Sex sex;
    public Trait_And_Magnitude[] traitsAndMagnitudes = new Trait_And_Magnitude[ 3 ];
    public uint startHydrationHours = 72;
    public uint startFoodHours = 72;

    public string bio;

    public uint hoursBelowForSlightlyThirsty = 48;
    public uint hoursBelowForThirsty = 24;
    public uint hoursBelowForVeryThirsty = 12;

    public uint hoursBelowForSlightlyHungry = 48;
    public uint hoursBelowForHungry = 24;
    public uint hoursBelowForVeryHungry = 12;

    public uint hoursGainedByHydrating = 10;
    public uint hoursGainedByFeeding = 10;

    // GUI
    public Sprite selectFromCharactersSprite;
    public Sprite selectCharacterSprite;
    public Sprite traitsSprite;
    public Sprite statusCharacterSprite;

    private Timestamp timeWhenDeadToThirst;
    private Timestamp timeWhenDeadToHunger;

    public Hydration hydration = Hydration.HYDRATED;
    public Hunger hunger = Hunger.WELL_FED;
    public Sickness sickness = Sickness.HEALTHY;
    public Injury injury = Injury.NOT_INJURED;
    public bool isDead = false;

	// Unity Callbacks -----------------------------
	void Awake ()
    {
        gameStateManager = GameObject.Find( "GameState_Manager" ).GetComponent< GameState_Manager >();
        timeManager = GameObject.Find( "Time_Manager" ).GetComponent< Time_Manager >();

        timeWhenDeadToThirst = timeManager.AddHoursToTimestamp( timeManager.GetTime(), startHydrationHours );
        timeWhenDeadToHunger = timeManager.AddHoursToTimestamp( timeManager.GetTime(), startFoodHours );
	}

	void Update ()
    {
        if( gameStateManager.GetState() == GameState.State.DAY )
        {
	        UpdateThirst();
            UpdateHunger();
        }
	}

    void UpdateThirst()
    {
        Timestamp currentTime = timeManager.GetTime();
        uint hoursUntilDeadFromThirst = timeManager.HoursBetweenTimestamps( currentTime, timeWhenDeadToThirst );
        if( hoursUntilDeadFromThirst == 0 )
        {
            Debug.Log( "Character died due to hydration" );
            Kill();
        }
        else if( hoursUntilDeadFromThirst < hoursBelowForVeryThirsty )
        {
            hydration = Hydration.VERY_THIRSTY;
        }
        else if( hoursUntilDeadFromThirst < hoursBelowForThirsty )
        {
            hydration = Hydration.THIRSTY;
        }
        else if( hoursUntilDeadFromThirst < hoursBelowForSlightlyThirsty )
        {
            hydration = Hydration.SLIGHTLY_THIRSTY;
        }
        else
        {
            hydration = Hydration.HYDRATED;
        }
    }

    void UpdateHunger()
    {
        Timestamp currentTime = timeManager.GetTime();
        uint hoursUntilDeadFromHunger = timeManager.HoursBetweenTimestamps( currentTime, timeWhenDeadToHunger );
        if( hoursUntilDeadFromHunger == 0 )
        {
            Debug.Log( "Character died due to hunger" );
            Kill();
        }
        else if( hoursUntilDeadFromHunger < hoursBelowForVeryHungry )
        {
            hunger = Hunger.VERY_HUNGRY;
        }
        else if( hoursUntilDeadFromHunger < hoursBelowForHungry )
        {
            hunger = Hunger.HUNGRY;
        }
        else if( hoursUntilDeadFromHunger < hoursBelowForSlightlyHungry )
        {
            hunger = Hunger.SLIGHTLY_HUNGRY;
        }
        else
        {
            hunger = Hunger.WELL_FED;
        }
    }

    public void UpgradeTrait( Trait trait )
    {
        for( uint index = 0; index < traitsAndMagnitudes.Length; ++index )
        {
            if( traitsAndMagnitudes[index].trait == trait )
            {
                traitsAndMagnitudes[index].magnitude += 1;
            }
        }
    }

    public void Hydrate( uint hours )
    {
        timeWhenDeadToThirst = timeManager.AddHoursToTimestamp( timeWhenDeadToThirst, hours );
    }

    public void Feed( uint hours )
    {
        timeWhenDeadToHunger = timeManager.AddHoursToTimestamp( timeWhenDeadToHunger, hours );
    }

    public Timestamp GetTimeWhenDeadToThirst()
    {
        return timeWhenDeadToThirst;
    }

    public Timestamp GetTimeWhenDeadToHunger()
    {
        return timeWhenDeadToHunger;
    }

    public Sickness GetSickness()
    {
        return sickness;
    }

    public void SetSickness( Sickness inSickness )
    {
        sickness = inSickness;
    }

    public Injury GetInjury()
    {
        return injury;
    }

    public void SetInjury( Injury inInjury )
    {
        injury = inInjury;
    }

    public Hydration GetHydration()
    {
        return hydration;
    }

    public Hunger GetHunger()
    {
        return hunger;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void Kill()
    {
        isDead = true;
        SceneManager.LoadScene( 0 );
    }
}
