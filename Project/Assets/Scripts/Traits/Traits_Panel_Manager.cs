using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Traits_Panel_Manager : MonoBehaviour
 {
    public Button trait0Button;
    public Button trait1Button;
    public Button trait2Button;

    public Text trait0Text;
    public Text trait1Text;
    public Text trait2Text;

    public Image characterImage;

    Character target = null;

    Traits_Manager traitsManager;

    void Awake()
    {
        traitsManager = GameObject.Find( "Traits_Manager" ).GetComponent<Traits_Manager>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if( HasTarget() )
        {
            trait0Text.text = target.traitsAndMagnitudes[0].magnitude.ToString() + "/" + 5;
            trait1Text.text = target.traitsAndMagnitudes[1].magnitude.ToString() + "/" + 5;
            trait2Text.text = target.traitsAndMagnitudes[2].magnitude.ToString() + "/" + 5;
        }	
	}

    public bool HasTarget()
    {
        return target != null;
    }

    public void TargetCharacter( Character character )
    {
        if( HasTarget() )
        {
            trait0Button.onClick.RemoveAllListeners();
            trait1Button.onClick.RemoveAllListeners();
            trait2Button.onClick.RemoveAllListeners();
        }

        target = character;
        
        Character.Trait trait0 = character.traitsAndMagnitudes[ 0 ].trait;
        Character.Trait trait1 = character.traitsAndMagnitudes[ 1 ].trait;
        Character.Trait trait2 = character.traitsAndMagnitudes[ 2 ].trait;

        trait0Button.image.sprite = traitsManager.traitsSprites[ (int)trait0 ];
        trait1Button.image.sprite = traitsManager.traitsSprites[ (int)trait1 ];
        trait2Button.image.sprite = traitsManager.traitsSprites[ (int)trait2 ];

        trait0Button.onClick.AddListener( () => TraitButtonClicked( trait0 ) );
        trait1Button.onClick.AddListener( () => TraitButtonClicked( trait1 ) );
        trait2Button.onClick.AddListener( () => TraitButtonClicked( trait2 ) );

        characterImage.sprite = character.traitsSprite;
    }

    void TraitButtonClicked( Character.Trait trait )
    {
        Transform canvas = GameObject.Find( "Canvas" ).GetComponent<Transform>();
        GameObject traitUpgradePopup = GameObject.Instantiate( traitsManager.traitUpgradePopupScreen[( int )trait], canvas, false );
        Screen_Upgrade_Trait_Popup_Manager traitUpgradePopupManager = traitUpgradePopup.GetComponent<Screen_Upgrade_Trait_Popup_Manager>();

        traitUpgradePopupManager.TargetCharacter( target );
        
    }
}
