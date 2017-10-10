using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Time_Manager : MonoBehaviour {

    GameState_Manager gameStateManager;
    Time_Manager timeManager;
    public Text dayText;
    public Text minuteText;

    void Awake()
    {
        timeManager = GameObject.Find( "Time_Manager" ).GetComponent<Time_Manager>();
    }

    void OnGUI()
    {
        Timestamp currentTime = timeManager.GetTime();
        uint currentHour = currentTime.hour;
        uint currentMinute = currentTime.minute;

        dayText.text = "Day " + currentTime.day;

        // Time
        string suffix = "";

        // Set hour
        string hour = "";

        if( currentHour > 12 )
        {
            suffix = "PM";
            hour = ( currentHour - 12 ).ToString();
        }
        else if( currentHour == 12 )
        {
            suffix = "PM";
            hour = currentHour.ToString();
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

        minuteText.text = hour + ":" + minute + " " + suffix;
    }

    // Use this for initialization
    public void ShowScreen()
    {
        gameObject.SetActive( true );       
    }

    public void HideScreen()
    {
        gameObject.SetActive( false );
    }
}
