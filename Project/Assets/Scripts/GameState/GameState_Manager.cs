using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_Manager : MonoBehaviour {

	// State ------------------------------------
    private GameState.State currentState = GameState.State.MAIN_TITLE;
    private GameState.State previousState;

    // Callback ---------------------------------
    public delegate void OnGameStateChangedCallback( GameState.State newState, GameState.State previousState );
    private List< OnGameStateChangedCallback > onChangedGameStateCallbacks = new List<OnGameStateChangedCallback>();

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
        previousState = currentState;
        currentState = newState;
        OnGameStateChanged();
    }

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
    }
}
