using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timestamp
{
    public uint day;
    public uint hour;
    public uint minute;

    public Timestamp( uint inDay, uint inHour, uint inMinute )
    {
        day = inDay;
        hour = inHour;
        minute = inMinute;
    }
}

public class Time_Manager : MonoBehaviour
{
    // References
    private GameState_Manager gameStateManager;
    public Screen_Time_Manager screenManager;

    // Day
    public uint startDay = 1;
    private uint currentDay;

    [Range(0, 23)]
    public uint startOfDayHour = 8;

    [Range(0, 60)]
    public uint startOfDayMinute = 30;

    [Range(0, 24)]
    public uint endOfDayHour = 22;

    [Range(0, 60)]
    public uint endOfDayMinute = 30;

    [Range(0,24)]
    public uint allowPlayerEndDayHour = 12;

    [Range(0, 60)]
    public uint allowPlayerEndDayMinute = 30;

    public float minutesPassedPerSecond = 1f;

    private uint currentHour;
    private uint currentMinute;
    private float accumulatedMinutes = 0f;

	/// Unity Callbacks
    void Awake ()
    {
        // Check values
        Debug.Assert( startOfDayHour < 24, "Starting day hour was greater than 24" );
        Debug.Assert( startOfDayMinute < 60, "Starting day minute was greater than 60" );
        Debug.Assert( endOfDayHour < 24, "Ending day hour was greater than 24" );
        Debug.Assert( endOfDayMinute < 60, "Ending day minute was greater than 60" );
    
        Debug.Assert( startOfDayHour < endOfDayHour, "Ending day hour is same as starting day hour" );

        // Start day
        currentDay = startDay;
        currentHour = startOfDayHour;
        currentMinute = startOfDayMinute;

        // References
        gameStateManager = GameObject.Find( "GameState_Manager" ).GetComponent< GameState_Manager >();
    }
	
	void Update ()
    {
        GameState.State state = gameStateManager.GetState();

        switch( state )
        {
            case GameState.State.HOME:
            case GameState.State.STATUS:
            case GameState.State.UPGRADE_HOME:
            case GameState.State.UPGRADE_TRAITS:
            case GameState.State.STORAGE:
            case GameState.State.CRAFTING:
            case GameState.State.WATER:
            case GameState.State.FARM:
                UpdateTime();
                UpdateDay();
                break;
        }

	}

    // Time -------------------------------------
    public Timestamp StartGameTime()
    {
        return new Timestamp( startDay, startOfDayHour, startOfDayMinute );
    }

    public uint MinutesBetweenTimestamps( Timestamp start, Timestamp end )
    {
        uint minutesBetween = 0;
        Timestamp temp = start;

        // days
        while( end.day - temp.day > 1 )
        {
            minutesBetween += 24 * 60;
            ++temp.day;
        }

        if( end.day - temp.day == 1 )
        {
            minutesBetween += 24 * 60;
            ++temp.day;

            while( temp.hour > end.hour )
            {
                minutesBetween -= 60;
                temp.hour -= 1;
            }

            while( temp.minute > end.minute )
            {
                minutesBetween -= end.minute - temp.minute;
                temp.minute = end.minute;
            }
        }

        // hours
        while( end.hour - temp.hour > 1 )
        {
            minutesBetween += 60;
            ++temp.hour;
        }

        if( end.hour - temp.hour == 1 && end.minute >= temp.minute )
        {
            minutesBetween += 60;
            ++temp.hour;

            
        }
        else if( end.hour - temp.hour == 1 && temp.minute > end.minute )
        {
            minutesBetween += 60 - temp.minute;
            temp.hour += 1;
            temp.minute = 0;
        }

        // minutes
        if( end.minute > temp.minute )
        {
            minutesBetween += end.minute - temp.minute;
            temp.minute = end.minute;
        }
        else if( end.minute < temp.minute )
        {
            minutesBetween += end.minute;
            temp.minute = end.minute;
        }

        if( temp.day != end.day || temp.hour != end.hour || temp.minute != end.minute )
        {
            throw new System.Exception( "Expected temp to equal end" );
        }
        
        return minutesBetween;
    }

    public uint HoursBetweenTimestamps( Timestamp start, Timestamp end )
    {
        uint hoursBetween = 0;
        Timestamp temp = start;
        
        // days
        while( end.day - temp.day > 1 )
        {
            hoursBetween += 24;
            ++temp.day;
        }

        if( end.day - temp.day == 1 )
        {
            hoursBetween += 24;
            ++temp.day;

            while( temp.hour > end.hour )
            {
                --hoursBetween;
                temp.hour -= 1;
            }
        }

        // hours
        while( end.hour - temp.hour > 1 )
        {
            ++hoursBetween;
            ++temp.hour;
        }

        if( end.hour - temp.hour == 1 && end.minute > temp.minute )
        {
            ++hoursBetween;
        }

        return hoursBetween;
    }

    public Timestamp AddHoursToTimestamp( Timestamp start, uint hours )
    {
        start.hour += hours;

        while( start.hour > 24 )
        {
            start.hour -= 24;
            ++start.day;
        }

        return start;
    }

    public Timestamp GetTime() 
    {
        return new Timestamp( currentDay, currentHour, currentMinute );
    }

    public void StartNewDay()
    {
        SetTimeToStartOfDay();
        ++currentDay;
    }

    private void SetTimeToStartOfDay()
    {
        currentHour = startOfDayHour;
        currentMinute = startOfDayMinute;  
    }

    private void UpdateTime()
    {
        accumulatedMinutes += minutesPassedPerSecond * Time.deltaTime;
        
        while( accumulatedMinutes > 1 )
        {
            ++currentMinute;
            --accumulatedMinutes;

            if( currentMinute >= 60 )
            {
                currentMinute -= 60;
                ++currentHour;
            }
        }
    }

    private void UpdateDay()
    {
        if( currentHour >= endOfDayHour && currentMinute >= endOfDayMinute )
        {
            EndDay();
        }
    }

    public void EndDay()
    {
        gameStateManager.ChangeState( GameState.State.SCAVENGE );
        StartNewDay();
    }

}
