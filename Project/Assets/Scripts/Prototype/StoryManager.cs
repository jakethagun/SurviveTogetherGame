using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Stored_Day_Story
{
    public Day_Story_Element story;

    public Stored_Day_Story( Day_Story_Element inStory )
    {
        story = inStory;
        //Debug.Log( "Added Story: " + story.name );
    }
};

public class StoryManager : MonoBehaviour 
{
    public Text text;
    private Dictionary< string, Stored_Day_Story > dayStories = new Dictionary<string, Stored_Day_Story>();

    // References
    public GameStateManager gameStateManager;
    public TimeManager timeManager;

    // Unity Callbacks --------------------------
	void Start () 
    {
	    gameStateManager.RegisterOnGameStateChanged( OnGameStateChanged );
        PopulateDayStories();
	}

	void Update () {
		
	}

    void PopulateDayStories()
    {
        var allFiles = Resources.LoadAll<UnityEngine.Object>( "Story/Day" );
        foreach (var obj in allFiles)
        {
            if (obj is GameObject)
            {
                GameObject go = obj as GameObject;
                if ( go.GetComponent<Day_Story_Element>() != null )
                {
                    Stored_Day_Story story = new Stored_Day_Story( go.GetComponent< Day_Story_Element >() );
                    string name = go.name;
                    dayStories.Add( name, story );
                }
            }
        }
    }

    void SetStoryForDay()
    {
        TimeManager.Timestamp currentTime = timeManager.GetTime();
        
        Day_Story_Element currentDayStory = dayStories[ currentTime.day.ToString() ].story;

        text.text = currentDayStory.story;
    }

    // GUI --------------------------------------

    // GameState --------------------------------
    void OnGameStateChanged(GameState.State newState, GameState.State previousState)
    {
        switch( newState )
        {
            case GameState.State.DAY:
                text.gameObject.SetActive( true );
                SetStoryForDay();
                break;
            case GameState.State.END_OF_DAY:
            default:
                text.gameObject.SetActive( false );
                break;
        }
    }

}
