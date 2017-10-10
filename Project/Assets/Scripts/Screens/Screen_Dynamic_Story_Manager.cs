using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Next_Screen
{
    public Button button;
    public GameObject template;
}

public class Screen_Dynamic_Story_Manager : MonoBehaviour
{
    public List<Next_Screen> nextScreenButtons = null;
    public Button destroyScreenButton = null;

    void Start()
    {
        if( nextScreenButtons != null )
        {
            foreach( Next_Screen nextScreen in nextScreenButtons )
            {
                GameObject template = nextScreen.template;
                nextScreen.button.onClick.AddListener( ()=>OnClickNextScreen( template ) );
            }
        }

        if( destroyScreenButton != null )
        {
            destroyScreenButton.onClick.AddListener( OnClickDestroy );
        }
    }

    void OnClickNextScreen( GameObject nextScreen )
    {
        GameObject.Instantiate( nextScreen, transform.parent, false );
        GameObject.Destroy( gameObject );
    }

    void OnClickDestroy()
    {
        GameObject.Destroy( gameObject );
    }
}
