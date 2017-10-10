using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Game_Story : MonoBehaviour
{
    public string[] pages;
    public Text page;
    public Button nextPage;
    uint currentPage = 0;

    GameState_Manager gameStateManager;

    // Use this for initialization
    void Start()
    {
        nextPage.onClick.AddListener( OnClickNext );
        gameStateManager = GameObject.Find( "GameState_Manager" ).GetComponent<GameState_Manager>();

        page.text = pages[0];
    }

    void OnClickNext()
    {
        if( currentPage >= pages.Length - 1 )
        {
            gameStateManager.ChangeState( GameState.State.SELECT_FROM_CHARACTERS );
        }
        else
        {
            ++currentPage;
            page.text = pages[currentPage];
        }
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
}
