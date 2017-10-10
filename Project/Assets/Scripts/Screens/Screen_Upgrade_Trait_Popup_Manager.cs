using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Upgrade_Trait_Popup_Manager : MonoBehaviour
{
    public Character.Trait trait;
    Traits_Manager traitsManager;
    Character target;
    public Button upgradeButton;
    bool clickedLastFrame = false;
    public Button backButton;

    void Awake()
    {
        traitsManager = GameObject.Find( "Traits_Manager" ).GetComponent<Traits_Manager>();
        upgradeButton.onClick.AddListener( OnClickUpgradeButton );
        backButton.onClick.AddListener( OnClickBackButton );

        uint traitPoints = traitsManager.traitPoints;

        if( traitPoints == 0 )
        {
            upgradeButton.interactable = false;
        }
    }

    void Update()
    {
    }

    void OnClickBackButton()
    {
        GameObject.Destroy( gameObject );
    }

    public void OnClickUpgradeButton()
    {
        target.UpgradeTrait( trait );
        traitsManager.traitPoints -= 1;
        GameObject.Destroy( gameObject );
    }

    public void TargetCharacter( Character inTarget )
    {
        target = inTarget;
    }
}
