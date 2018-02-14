using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Home_Manager : MonoBehaviour
{
    // References
    public Button statusButton;
    public Button upgradesButton;
    public Button storageButton;
    public Button craftingButton;
    public Button waterButton;
    public Button farmButton;

    public Button endDayButton;

    public Text dailyStory;

    public Transform dynamicStoryParent;

    Time_Manager timeManager;
    Story_Manager storyManager;
    GameState_Manager gameStateManager;

	// Use this for initialization
	void Awake ()
    {
	    statusButton.onClick.AddListener( ()=>ChangeState( GameState.State.STATUS ) );
        upgradesButton.onClick.AddListener( () => ChangeState( GameState.State.UPGRADE_HOME ) );
        storageButton.onClick.AddListener( () => ChangeState( GameState.State.STORAGE ) );
        craftingButton.onClick.AddListener( () => ChangeState( GameState.State.CRAFTING ) );
        waterButton.onClick.AddListener( () => ChangeState( GameState.State.WATER ) );
        farmButton.onClick.AddListener( () => ChangeState( GameState.State.FARM ) );

        gameStateManager = GameObject.Find( "GameState_Manager" ).GetComponent< GameState_Manager >();
        timeManager = GameObject.Find( "Time_Manager" ).GetComponent<Time_Manager>();
        storyManager = GameObject.Find( "Story_Manager" ).GetComponent<Story_Manager>();

        endDayButton.onClick.AddListener( timeManager.EndDay );
    }
	
	// Update is called once per frame
	void Update ()
    {
        Timestamp currentTime = timeManager.GetTime();

        if( !endDayButton.isActiveAndEnabled )
        {
            if( currentTime.hour > timeManager.allowPlayerEndDayHour )
            {
                endDayButton.gameObject.SetActive( true );
            }
            else if( currentTime.hour == timeManager.allowPlayerEndDayHour && currentTime.minute > timeManager.allowPlayerEndDayMinute )
            {
                endDayButton.gameObject.SetActive( true );
            }
        }

        
    }

    /*
    private void LateUpdate()
    {
        if( dynamicStoryParent.childCount == 0 && storyManager.DynamicStoryAvailable() )
        {
            GameObject.Instantiate( storyManager.GetDynamicStory(), dynamicStoryParent, false );
        }
    }
    */

    private void ChangeState( GameState.State state )
    {
        gameStateManager.ChangeState( state );
    }

    public void HideScreen()
    {
        gameObject.SetActive( false );
    }

    public void ShowScreen()
    {
        gameObject.SetActive( true );
        Timestamp currentTime = timeManager.GetTime();

        if( storyManager.DayStoryAvailable() )
        {
            dailyStory.text = storyManager.GetDayStory().story;
        }

        endDayButton.gameObject.SetActive( false );
    }
}
