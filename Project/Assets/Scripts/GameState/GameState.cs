// RATIONALE ------------------------------------
// * Supporting classes for gamestate

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    // State ------------------------------------
    public enum State
    {
        MAIN_TITLE,
        GAME_STORY,
        SELECT_FROM_CHARACTERS,
        SELECT_CHARACTER,
        HOME,
        STATUS,
        UPGRADE_HOME,
        UPGRADE_TRAITS,
        STORAGE,
        CRAFTING,
        WATER,
        FARM,
        SCAVENGE,
        SCAVENGED,

        CHARACTER_SELECT,
        DAY,
        NIGHT,
        SCAVENGING,
        DEFENDING,
        RESTING,
        END_OF_DAY
    }

    static public State Next( State state )
    {
        switch( state )
        {
            case State.CHARACTER_SELECT:
                return State.DAY;
            case State.DAY:
                return State.END_OF_DAY;
            case State.STATUS:
                 return State.END_OF_DAY;
            default:
                Debug.LogError( "Next state was not valid" );
                break;
        }

        return State.CHARACTER_SELECT;
    }
}
