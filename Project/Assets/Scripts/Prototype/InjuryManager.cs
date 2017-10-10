using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjuryManager : MonoBehaviour {

	public uint hoursBelowForSlightlyInjured = 48;
    public uint hoursBelowForInjured = 24;
    public uint hoursBelowForVeryInjured = 12;

    [Range(0.0f,100.0f)]
    public float chanceToDieAtSlightlyInjured = 0;

    [Range(0.0f,100.0f)]
    public float chanceToDieAtInjured = 0;

    [Range(0.0f,100.0f)]
    public float chanceToDieAtVeryInjured = 0;

    // References
    public GameStateManager gameStateManager;
    public CharacterManager characterManager;
    public FoodManager foodManager;
    public TimeManager timeManager;

    // Unity Callbacks --------------------------
	void Start () {
		
	}
	
	void Update () 
    {
        if( gameStateManager.GetState() == GameState.State.DAY )
        {
           
        }
    }

    public void InjureCharacter()
    {
        Character character = characterManager.GetCharacter();

        if( character.GetInjury() == Character.Injury.VERY_INJURED )
        {
            Debug.Log( "Death : due to injury exceeding very injured" );
            character.Kill();
        }
        else
        {
            float random = Random.Range( 0.0f, 100.0f );

            character.SetInjury( character.GetInjury() + 1 );
            switch( character.GetInjury() )
            {
                case Character.Injury.SLIGHTLY_INJURED:
                    if( chanceToDieAtSlightlyInjured > random )
                    {
                        Debug.Log( "Death : due to injury slightly injured" );
                        character.Kill();
                    }
                    break;
                case Character.Injury.INJURED:
                    if( chanceToDieAtInjured > random )
                    {
                        Debug.Log( "Death : due to injury injured" );
                        character.Kill();
                    }
                    break;
                case Character.Injury.VERY_INJURED:
                    if( chanceToDieAtVeryInjured > random )
                    {
                        Debug.Log( "Death : due to injury very injured" );
                        character.Kill();
                    }
                    break;
            }
            
        }
        
        
        
    }
}
