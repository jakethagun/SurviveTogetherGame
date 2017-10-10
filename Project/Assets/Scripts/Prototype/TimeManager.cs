// RATIONALE ------------------------------------
// * Manages what day it is
// * Manages what time it is
// * Manages GUI related to day and time

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    // PODS
    public struct Timestamp
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

    // References
    public GameStateManager gameStateManager;

    // GUI Elements
    public Text day;
    public Text time;
    public GameObject endDay;
    public Button endDayButton;

    // Day
    public uint startDay = 1;
    private uint currentDay;

    // Time
    public bool twentyFourHourTime = true;

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

    // Unity Callbacks
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

        // GUI
        DisableEndDayGUI();
        DisableDayTimeGUI();
        endDayButton.onClick.AddListener( OnEndDay );

        // GameState
        gameStateManager.RegisterOnGameStateChanged( OnGameStateChanged );
    }
	
	void Update ()
    {
        if( gameStateManager.GetState() == GameState.State.DAY )
        {
            if( currentHour > allowPlayerEndDayHour )
            {
                EnableEndDayGUI();
            }
            else if( currentHour == allowPlayerEndDayHour && currentMinute >= allowPlayerEndDayMinute )
            {
                EnableEndDayGUI();
            }

            UpdateTime();
            UpdateDay();
        }
	}

    void OnGUI()
    {
        SetTimeText();
        SetDayText();
    }

    // Time -------------------------------------
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
            DisableEndDayGUI();
            EndDay();
        }
    }

    private void EndDay()
    {
        gameStateManager.ChangeState( GameState.State.NIGHT );
    }

    // GUI --------------------------------------
    private void EnableDayTimeGUI()
    {
        day.enabled = true;
        time.enabled = true;
    }

    private void DisableDayTimeGUI()
    {
        day.enabled = false;
        time.enabled = false;
    }

    private void EnableEndDayGUI() 
    {
        endDay.SetActive( true );
    }

    private void DisableEndDayGUI() 
    {
        endDay.SetActive( false );
    }

    private void SetDayText()
    {
        day.text = "Day " + currentDay.ToString();
    }

    private void SetTimeText()
    {
        if( twentyFourHourTime )
        {
            string hour = currentHour.ToString();

            // Set minute
            string minute = "";

            if( currentMinute < 10 )
            {
                minute = "0";
            }

            minute += currentMinute.ToString();

            time.text = hour + ":" + minute;
        }
        else 
        {
            string suffix = "";
            
            // Set hour
            string hour = "";

            if( currentHour > 12 )
            {
                suffix = "PM";
                hour = (currentHour - 12).ToString();
            }
            else
            {
                suffix = "AM";
                hour = currentHour.ToString();
            }

            // Set minute
            string minute = "";

            if( currentMinute < 10 )
            {
                minute = "0";
            }

            minute += currentMinute.ToString();

            time.text = hour + ":" + minute + " " + suffix;            
        }
    }

    private void OnEndDay()
    {
        EndDay();
    }

    // GameState --------------------------------
    void OnGameStateChanged(GameState.State newState, GameState.State previousState)
    {
        switch( newState )
        {
            case GameState.State.DAY:
                EnableDayTimeGUI();
                break;
            default:
                DisableDayTimeGUI();
                DisableEndDayGUI();
                break;
        }
    }
}
