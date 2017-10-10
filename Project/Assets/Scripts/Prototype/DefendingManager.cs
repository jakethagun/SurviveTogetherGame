using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefendingManager : MonoBehaviour
{
    public uint minimumMaterialsLost = 0;
    public uint maximumMaterialsLost = 10;

    public uint minimumCharacterInjuries = 0;
    public uint maximumCharacterInjuries = 1;

    public uint minimumSurvivorsDieing = 0;
    public uint maximumSurvivorsDieing = 5;

    // GUI
    public Button defendButton;

    // References
    public GameStateManager gameStateManager;
    public TimeManager timeManager;
    public StorageManager storageManager;
    public InjuryManager injuryManager;
    public SurvivorsManager survivorsManager;
    public CharacterManager characterManager;

	// Unity Callbacks -----------------------------
	void Start ()
    {
        DisableDefendButton();

        defendButton.onClick.AddListener( OnDefendButtonClicked );

	    // GameState
        gameStateManager.RegisterOnGameStateChanged( OnGameStateChanged );	
	}

	void Update ()
    {
	}

    void DisableDefendButton()
    {
        defendButton.gameObject.SetActive( false );
    }

    void EnableDefendButton()
    {
        defendButton.gameObject.SetActive( true );
    }

    void OnDefendButtonClicked()
    {
        GameState.State state = gameStateManager.GetState();
        
        if( state != GameState.State.DEFENDING )
        {
            gameStateManager.ChangeState( GameState.State.DEFENDING );

            RaidResult result = Raid( minimumMaterialsLost, maximumMaterialsLost, minimumCharacterInjuries, maximumCharacterInjuries, minimumSurvivorsDieing, maximumSurvivorsDieing );
            result.PrintResult();
            

            gameStateManager.ChangeState( GameState.State.END_OF_DAY );
        }
    }

    
    public class RaidResult
    {
        public uint materialsLost = 0;
        public uint characterInjuries = 0;
        public uint survivorsDied = 0;
    
        public List< Item > itemsLost = new List< Item >();

        public void PrintResult()
        {
            Debug.Log( "Raid Result: " + "Materials Lost: " + materialsLost.ToString() + " Character Injuries: " + characterInjuries + " Survivors Died: " + survivorsDied );

            foreach( var itemLost in itemsLost )
            {
                Debug.Log( "Material lost in raid: " + itemLost.name );
            }
        }
    };

    public RaidResult Raid( uint inMinimumMaterialsLost, uint inMaximumMaterialsLost, uint inMinimumCharacterInjuries, uint inMaximumCharacterInjuries, uint inMinimumSurvivorDeaths, uint inMaximumSurvivorDeaths )
    {
        RaidResult result = new RaidResult();
        int random;
        
        // materials
        random = Random.Range( (int)inMinimumMaterialsLost, (int)inMaximumMaterialsLost + 1 );
        for( uint i = 0; storageManager.ItemCount() != 0 && i < random; ++i )
        {
            Item lostItem = storageManager.DropRandomItem();
            result.itemsLost.Add( lostItem );
            result.materialsLost += 1;
        }
        
        // injury
        random = Random.Range( (int)inMinimumCharacterInjuries, (int)inMaximumCharacterInjuries + 1 );
        for( uint i = 0; characterManager.GetCharacter().IsDead() == false && i < random; ++i )
        {
            injuryManager.InjureCharacter();
            result.characterInjuries += 1;
        }
             
        // kill survivor
        random = Random.Range( (int)inMinimumSurvivorDeaths, (int)inMaximumSurvivorDeaths + 1 );
        for( uint i = 0; survivorsManager.GetNumberOfSurvivors() > 0 && i < random; ++i )
        {
            survivorsManager.KillRandom();
            result.survivorsDied += 1;
        }

        return result;
    }

    // GameState --------------------------------
    void OnGameStateChanged(GameState.State newState, GameState.State previousState)
    {
        switch( newState )
        {
            case GameState.State.NIGHT:
                EnableDefendButton();
                break;
            default:
                DisableDefendButton();
                break;
        }
    }
}
