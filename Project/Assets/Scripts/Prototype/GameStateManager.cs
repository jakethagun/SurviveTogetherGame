// RATIONALE ------------------------------------
// * Manages the gamestate

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    // References -------------------------------
    public ForegroundManager foregroundManager;
    public TimeManager timeManager;
        
    // State ------------------------------------
    private GameState.State currentState = GameState.State.CHARACTER_SELECT;
    private GameState.State previousState;

    public GameState.State GetState()
    {
        return currentState;
    }

    public void TransitionToNextState()
    {
        ChangeState( GameState.Next( currentState ) );
    }

    public void ChangeState( GameState.State newState )
    {
        switch( newState )
        {
            case GameState.State.DAY:
                if( foregroundManager.GetState() == ForegroundManager.State.IN )
                    foregroundManager.FadeOut();
                break;
            case GameState.State.END_OF_DAY:
                if( foregroundManager.GetState() == ForegroundManager.State.OUT )
                    foregroundManager.FadeIn();
                break;
        }

        previousState = currentState;
        currentState = newState;
        OnGameStateChanged();
    }

    // OnChangedGameState -----------------------
    public delegate void OnGameStateChangedCallback( GameState.State newState, GameState.State previousState );
    private List< OnGameStateChangedCallback > onChangedGameStateCallbacks = new List<OnGameStateChangedCallback>();

    void OnGameStateChanged()
    {
        foreach( var callback in onChangedGameStateCallbacks )
        {
            callback( currentState, previousState );
        }
    }

    public void RegisterOnGameStateChanged( OnGameStateChangedCallback callback )
    {
        onChangedGameStateCallbacks.Add( callback );
    }

    // Unity Callbacks ---------------------------
    void Start()
    {
    }

    void Update()
    {
        switch( currentState )
        {
            case GameState.State.END_OF_DAY:
                if( foregroundManager.GetState() == ForegroundManager.State.IN )
                    timeManager.StartNewDay();
                    foregroundManager.FadeOut();
                if( foregroundManager.GetState() == ForegroundManager.State.OUT )
                    ChangeState( GameState.State.DAY );
                break;
        }
    }



}
