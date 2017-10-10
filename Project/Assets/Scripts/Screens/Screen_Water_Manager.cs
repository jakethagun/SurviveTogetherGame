using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Water_Manager : MonoBehaviour
{
    public Text rateText;
    public Text totalText;

    public Image foregroundImage;

    public Button backButton;

    GameState_Manager gameStateManager;

    Water_Manager waterManager;
    Color defaultForegroundColor;

    void Awake()
    {
        waterManager = GameObject.Find( "Water_Manager" ).GetComponent<Water_Manager>();
        gameStateManager = GameObject.Find( "GameState_Manager" ).GetComponent<GameState_Manager>();
        backButton.onClick.AddListener( OnBackButtonClicked );

        defaultForegroundColor = foregroundImage.color;
    }

    void OnBackButtonClicked()
    {
        gameStateManager.ChangeState( GameState.State.HOME );
    }

    void Refresh()
    {
        float currentWater = waterManager.GetCurrentWater();

        float waterSystemQuantity = waterManager.GetWaterSystemQuantity();
        float waterSystemRate = waterManager.GetWaterSystemRatePerHour();
        float waterSystemMaximum = waterManager.GetWaterSystemMaximum();

        totalText.text = ( ( int )currentWater ).ToString();

        rateText.text = "Rate : " + ( ( int )waterSystemRate ).ToString();

        foregroundImage.fillAmount = waterSystemQuantity / waterSystemMaximum;

        if( waterSystemQuantity / waterSystemMaximum > 0.99f )
        {
            if( foregroundImage.gameObject.GetComponent<Button>() == null )
            {
                Button harvest = foregroundImage.gameObject.AddComponent<Button>();
                harvest.onClick.AddListener( OnClickedHarvestWater );
                foregroundImage.color = Color.green;
            }
        }
        else
        {
            foregroundImage.color = defaultForegroundColor;
        }
    }

    void OnClickedHarvestWater()
    {
        waterManager.HarvestWater();
        Destroy( foregroundImage.gameObject.GetComponent<Button>() );
    }

    void Update()
    {
        Refresh();
    }
    
    public void ShowScreen()
    {
        gameObject.SetActive( true );
        Refresh();
    }

    public void HideScreen()
    {
        gameObject.SetActive( false );
    }
}
