using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Stored_Scavenging_Location
{
    public ScavengingLocation location;

    public Stored_Scavenging_Location( ScavengingLocation inLocation )
    {
        location = inLocation;
    }
};

public class ScavengingManager : MonoBehaviour 
{
    private Dictionary< string, Stored_Scavenging_Location > scavengingLocations = new Dictionary<string, Stored_Scavenging_Location>();

    // GUI
    public Button scavengeButton;

    // References
    public GameStateManager gameStateManager;
    public TimeManager timeManager;

	// Unity Callbacks -----------------------------
	void Start ()
    {
        PopulateScavengingLocations();
        DisableScavengeButton();

        scavengeButton.onClick.AddListener( OnScavengeButtonClicked );

	    // GameState
        gameStateManager.RegisterOnGameStateChanged( OnGameStateChanged );	
	}

	void Update ()
    {
	}

    void DisableScavengeButton()
    {
        scavengeButton.gameObject.SetActive( false );
    }

    void EnableScavengeButton()
    {
        scavengeButton.gameObject.SetActive( true );
    }

    void PopulateScavengingLocations()
    {
        var allFiles = Resources.LoadAll<UnityEngine.Object>( "Scavenging_Locations" );
        foreach (var obj in allFiles)
        {
            if (obj is GameObject)
            {
                GameObject go = obj as GameObject;
                if ( go.GetComponent<ScavengingLocation>() != null )
                {
                    Stored_Scavenging_Location location = new Stored_Scavenging_Location( go.GetComponent< ScavengingLocation >() );
                    string name = go.name;
                    scavengingLocations.Add( name, location );
                }
            }
        }
    }

    void OnScavengeButtonClicked()
    {
        GameState.State state = gameStateManager.GetState();
        
        if( state != GameState.State.SCAVENGING )
        {
            gameStateManager.ChangeState( GameState.State.SCAVENGING );

            int random = Random.Range( 0, scavengingLocations.Count );

            foreach( var scavengingLocation in scavengingLocations )
            {
                if( random == 0 )
                {
                    ScavengingLocation.ScavengeResult result = scavengingLocation.Value.location.Scavenge();
                    result.PrintResult();
                    break;
                }
                else
                {
                    --random;
                }
            }

            gameStateManager.ChangeState( GameState.State.END_OF_DAY );
        }
    }

    // GameState --------------------------------
    void OnGameStateChanged(GameState.State newState, GameState.State previousState)
    {
        switch( newState )
        {
            case GameState.State.NIGHT:
                EnableScavengeButton();
                break;
            default:
                DisableScavengeButton();
                break;
        }
    }
}
