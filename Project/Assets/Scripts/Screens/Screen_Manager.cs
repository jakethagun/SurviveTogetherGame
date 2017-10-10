using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen_Manager : MonoBehaviour {

    // Screens
    public Screen_Main_Title mainTitleScreen;
    public Screen_Game_Story gameStoryScreen;
    public Screen_Select_From_Characters_Manager selectFromCharactersScreen;
    public Screen_Select_Character_Manager selectCharacterScreen;
    public Screen_Home_Manager homeScreen;
    public Screen_Status_Manager statusScreen;
    public Screen_Upgrades_Manager upgradesScreen;
    public Screen_Upgrade_Traits_Manager upgradeTraitsScreen;
    public Screen_Upgrade_Home_Manager upgradeHomeScreen;
    public Screen_Time_Manager timeScreen;
    public Screen_Storage_Manager storageScreen;
    public Screen_Crafting_Manager craftingScreen;
    public Screen_Water_Manager waterScreen;
    public Screen_Farm_Manager farmScreen;
    public Screen_Scavenge_Manager scavengeScreen;
    public Screen_Scavenged_Manager scavengedScreen;

    // References
    GameState_Manager gameStateManager;

	// Use this for initialization
	void Start ()
    {
	    gameStateManager = GameObject.Find( "GameState_Manager" ).GetComponent< GameState_Manager >();
        gameStateManager.RegisterOnGameStateChanged( OnGameStateChanged );
        mainTitleScreen.ShowScreen();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGameStateChanged(GameState.State newState, GameState.State previousState)
    { 
        switch( previousState )
        {
            case GameState.State.MAIN_TITLE:
                mainTitleScreen.HideScreen();
                break;
            case GameState.State.GAME_STORY:
                gameStoryScreen.HideScreen();
                break;
            case GameState.State.SELECT_FROM_CHARACTERS:
                selectFromCharactersScreen.HideScreen();
                break;
            case GameState.State.SELECT_CHARACTER:
                selectCharacterScreen.HideScreen();
                break;
            case GameState.State.HOME:
                homeScreen.HideScreen();
                break;
            case GameState.State.STATUS:
                statusScreen.HideScreen();
                break;
            case GameState.State.UPGRADE_HOME:
                upgradesScreen.HideScreen();
                upgradeHomeScreen.HideScreen();
                break;
            case GameState.State.UPGRADE_TRAITS:
                upgradesScreen.HideScreen();
                upgradeTraitsScreen.HideScreen();
                break;
            case GameState.State.STORAGE:
                storageScreen.HideScreen();
                break;
            case GameState.State.CRAFTING:
                craftingScreen.HideScreen();
                break;
            case GameState.State.WATER:
                waterScreen.HideScreen();
                break;
            case GameState.State.FARM:
                farmScreen.HideScreen();
                break;
            case GameState.State.SCAVENGE:
                scavengeScreen.HideScreen();
                break;
            case GameState.State.SCAVENGED:
                scavengedScreen.HideScreen();
                break;
        }

        switch( newState )
        {
            case GameState.State.MAIN_TITLE:
                mainTitleScreen.ShowScreen();
                break;
            case GameState.State.GAME_STORY:
                gameStoryScreen.ShowScreen();
                break;
            case GameState.State.SELECT_FROM_CHARACTERS:
                selectFromCharactersScreen.ShowScreen();
                break;
            case GameState.State.SELECT_CHARACTER:
                string characterSelected = selectFromCharactersScreen.GetCharacterSelected();
                selectCharacterScreen.ShowScreen( characterSelected );
                break;
            case GameState.State.HOME:
                homeScreen.ShowScreen();
                timeScreen.ShowScreen();
                break;
            case GameState.State.STATUS:
                statusScreen.ShowScreen();
                break;
            case GameState.State.UPGRADE_HOME:
                upgradesScreen.ShowScreen();
                upgradeHomeScreen.ShowScreen();
                break;
            case GameState.State.UPGRADE_TRAITS:
                upgradesScreen.ShowScreen();
                upgradeTraitsScreen.ShowScreen();
                break;
            case GameState.State.STORAGE:
                storageScreen.ShowScreen();
                break;
            case GameState.State.CRAFTING:
                craftingScreen.ShowScreen();
                break;
            case GameState.State.WATER:
                waterScreen.ShowScreen();
                break;
            case GameState.State.FARM:
                farmScreen.ShowScreen();
                break;
            case GameState.State.SCAVENGE:
                scavengeScreen.ShowScreen();
                break;
            case GameState.State.SCAVENGED:
                scavengedScreen.ShowScreen();
                break;
        }
    }
}
