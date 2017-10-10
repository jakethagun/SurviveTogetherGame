using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SicknessManager : MonoBehaviour 
{
    public uint hoursBelowForSlightlySick = 48;
    public uint hoursBelowForSick = 24;
    public uint hoursBelowForVerySick = 12;

    [Range(0.0f,100.0f)]
    public float chanceToDieAtSlightlySick = 0;

    [Range(0.0f,100.0f)]
    public float chanceToDieAtSick = 0;

    [Range(0.0f,100.0f)]
    public float chanceToDieAtVerySick = 0;

    // References
    public GameStateManager gameStateManager;
    public CharacterManager characterManager;
    public FoodManager foodManager;
    public Time_Manager timeManager;

    // Unity Callbacks --------------------------
	void Start () {
		
	}
	
	void Update () 
    {
        if( gameStateManager.GetState() == GameState.State.DAY )
        {
            Character character = characterManager.GetCharacter();
            Character.Sickness sickness = character.GetSickness();

            Timestamp timeWhenDeadToHunger = character.GetTimeWhenDeadToHunger();

            Timestamp currentTime = timeManager.GetTime();
            uint hoursUntilDeadFromHunger = timeManager.HoursBetweenTimestamps( currentTime, timeWhenDeadToHunger );

            if( hoursUntilDeadFromHunger < hoursBelowForVerySick && sickness < Character.Sickness.VERY_SICK )
            {
                character.SetSickness( Character.Sickness.VERY_SICK );
                
                float random = Random.Range( 0.0f, 100.0f );

                if( chanceToDieAtVerySick > random )
                {
                    character.Kill();
                }                
            }
            else if( hoursUntilDeadFromHunger < hoursBelowForSick && sickness < Character.Sickness.SICK )
            {
                character.SetSickness( Character.Sickness.SICK );

                float random = Random.Range( 0.0f, 100.0f );

                if( chanceToDieAtSick > random )
                {
                    character.Kill();
                } 
            }
            else if( hoursUntilDeadFromHunger < hoursBelowForSlightlySick && sickness < Character.Sickness.SLIGHTLY_SICK )
            {
                character.SetSickness( Character.Sickness.SLIGHTLY_SICK );

                float random = Random.Range( 0.0f, 100.0f );

                if( chanceToDieAtSlightlySick > random )
                {
                    character.Kill();
                }
            }
        }
    }
}
