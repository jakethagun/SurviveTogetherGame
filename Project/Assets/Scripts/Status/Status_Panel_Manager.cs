using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status_Panel_Manager : MonoBehaviour
{
    // find images for charcter, food, 
    public Image portraitImage;
    public Image hungerImage;
    public Image hydratedImage;
    public Image injuryImage;
    public Image sicknessImage;

    bool hasTarget = false;

    Status_Manager statusManager;

    void Awake()
    {
        statusManager = GameObject.Find( "Status_Manager" ).GetComponent<Status_Manager>();
    }

    public bool HasTarget()
    {
        return hasTarget;
    }

    public void TargetCharacter( Character character )
    {
        hasTarget = true;
        
        Character.Hunger hunger = character.GetHunger();
        Character.Hydration hydration = character.GetHydration();
        Character.Injury injury = character.GetInjury();
        Character.Sickness sickness = character.GetSickness();

        hungerImage.sprite = statusManager.hungerStateSprites[ (int)hunger ];
        hydratedImage.sprite = statusManager.hydratedStateSprites[ (int)hydration ];
        injuryImage.sprite = statusManager.injuryStateSprites[ (int)injury ];
        sicknessImage.sprite = statusManager.sicknessStateSprites[ (int)sickness ];
        portraitImage.sprite = character.statusCharacterSprite;
    }

    //public void TakeControlOfSurvivor( Survivor survivor );
}
