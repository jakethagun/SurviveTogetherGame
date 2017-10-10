using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Main_Title : MonoBehaviour
{
    public Button playButton;

    GameState_Manager gameStateManager;

    private void Awake()
    {
        gameStateManager = GameObject.Find( "GameState_Manager" ).GetComponent< GameState_Manager >();
    }

    // Use this for initialization
    void Start()
    {
        playButton.onClick.AddListener( OnPlayButtonClicked );
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowScreen()
    {
        gameObject.SetActive( true );
    }

    public void HideScreen()
    {
        gameObject.SetActive( false );
    }

    void OnPlayButtonClicked()
    {
        gameStateManager.ChangeState( GameState.State.GAME_STORY );
    }
}
