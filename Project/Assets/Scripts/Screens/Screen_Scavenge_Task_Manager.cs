using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Scavenge_Task_Manager : MonoBehaviour
{
    public Image characterPortrait;
    public Image hungerImage;
    public Image thirstImage;
    public Image injuryImage;
    public Image sicknessImage;

    public Button scavengeButton;
    public Button defendButton;

    public GameObject templateWeaponButton;
    public Transform primaryWeaponsParent;
    public Transform secondaryWeaponsParent;

    Item primaryWeapon = null;
    Item secondaryWeapon = null;

    Status_Manager statusManager;
    Weapons_Manager weaponsManager;

    Dictionary<string, Button> primaryWeaponButtons = new Dictionary<string, Button>();
    Dictionary<string, Button> secondaryWeaponButtons = new Dictionary<string, Button>();
    Dictionary<string, int> cachedEquipped = new Dictionary<string, int>();

    public enum Task
    {
        SCAVENGE,
        DEFEND,
        NONE
    };

    Task task = Task.NONE;

    // Use this for initialization
    void Awake()
    {
        statusManager = GameObject.Find( "Status_Manager" ).GetComponent<Status_Manager>();
        weaponsManager = GameObject.Find( "Weapons_Manager" ).GetComponent<Weapons_Manager>();

        scavengeButton.onClick.AddListener( () => SetTaskAndButton( Task.SCAVENGE ) );
        defendButton.onClick.AddListener( () => SetTaskAndButton( Task.DEFEND ) );

        PopulateWeapons();
    }

    void Update()
    {
        RefreshWeapons();
    }

    void RefreshWeapons()
    {
        List<Equipped_Weapon> weaponUpdate = weaponsManager.Weapons();

        foreach( Equipped_Weapon updatedWeapon in weaponUpdate )
        {
            string name = updatedWeapon.itemAndQuantity.item.name;

            bool updated = updatedWeapon.equipped != cachedEquipped[name];

            

            if( updated )
            {
                Button primaryWeaponButton = primaryWeaponButtons[name];
                Button secondaryWeaponButton = secondaryWeaponButtons[name];

                if( updatedWeapon.equipped == updatedWeapon.itemAndQuantity.quantity )
                {
                    if( primaryWeapon == null || name != primaryWeapon.name )
                    {
                        primaryWeaponButton.interactable = false;
                        primaryWeaponButton.image.color = Color.black;
                    }

                    if( secondaryWeapon == null || name != secondaryWeapon.name )
                    {
                        secondaryWeaponButton.interactable = false;
                        secondaryWeaponButton.image.color = Color.black;
                    }
                }
                else
                {
                    if( primaryWeapon == null || name != primaryWeapon.name )
                    {
                        primaryWeaponButton.interactable = true;
                        primaryWeaponButton.image.color = Color.white;
                    }

                    if( secondaryWeapon == null || name != secondaryWeapon.name )
                    {
                        secondaryWeaponButton.interactable = true;
                        secondaryWeaponButton.image.color = Color.white;
                    }
                }

                cachedEquipped[name] = updatedWeapon.equipped;
            }
        }
    }

    void PopulateWeapons()
    {
        List< Equipped_Weapon > weapons = weaponsManager.Weapons();

        foreach( Equipped_Weapon weapon in weapons )
        {
            GameObject weaponButton = GameObject.Instantiate( templateWeaponButton, primaryWeaponsParent, false );
            Button button = weaponButton.GetComponent<Button>();
            button.image.sprite = weapon.itemAndQuantity.item.storageSprite;

            if( primaryWeapon != null && weapon.itemAndQuantity.item.name == primaryWeapon.name )
            {
                button.image.color = Color.red;
            }

            Item weaponItem = weapon.itemAndQuantity.item;
            button.onClick.AddListener( () => EquipPrimaryWeapon( weaponItem ) );

            primaryWeaponButtons.Add( weaponItem.name, button );
            cachedEquipped.Add( weaponItem.name, weapon.equipped );
        }

        foreach( Equipped_Weapon weapon in weapons )
        {
            GameObject weaponButton = GameObject.Instantiate( templateWeaponButton, secondaryWeaponsParent, false );
            Button button = weaponButton.GetComponent<Button>();
            button.image.sprite = weapon.itemAndQuantity.item.storageSprite;

            if( secondaryWeapon != null && weapon.itemAndQuantity.item.name == secondaryWeapon.name )
            {
                button.image.color = Color.red;
            }

            Item weaponItem = weapon.itemAndQuantity.item;
            button.onClick.AddListener( () => EquipSecondaryWeapon( weaponItem ) );

            secondaryWeaponButtons.Add( weaponItem.name, button );
        }

        RefreshWeapons();
    }

    void EquipPrimaryWeapon( Item weapon )
    {
        if( primaryWeapon != null )
        {
            primaryWeaponButtons[primaryWeapon.name].image.color = Color.white;
        }

        if( primaryWeapon != null && weapon.name == primaryWeapon.name )
        {
            weaponsManager.Unequip( weapon );
            primaryWeapon = null;
            return;
        }
        else
        {
            weaponsManager.Equip( weapon );

            if( primaryWeapon != null )
            {
                weaponsManager.Unequip( primaryWeapon );
            }

            primaryWeapon = weapon;

            primaryWeaponButtons[weapon.name].image.color = Color.red;
        }
    }

    void EquipSecondaryWeapon( Item weapon )
    {
        if( secondaryWeapon != null )
        {
            secondaryWeaponButtons[secondaryWeapon.name].image.color = Color.white;
        }

        if( secondaryWeapon != null && weapon.name == secondaryWeapon.name )
        {
            weaponsManager.Unequip( secondaryWeapon );
            secondaryWeapon = null;
            return;
        }
        else
        {
            weaponsManager.Equip( weapon );

            if( secondaryWeapon != null )
            {
                weaponsManager.Unequip( secondaryWeapon );
            }

            secondaryWeapon = weapon;

            secondaryWeaponButtons[weapon.name].image.color = Color.red;
        }
    }

    void SetTaskAndButton( Task newTask )
    {
        Button previousSelected = null;
        Button selected = null;

        switch( task )
        {
            case Task.SCAVENGE:
                previousSelected = scavengeButton;
                break;
            case Task.DEFEND:
                previousSelected = defendButton;
                break;
            case Task.NONE:
                previousSelected = null;
                break;
            default:
                throw new System.Exception( "Unhandled task" );
        }

        task = newTask;

        switch( newTask )
        {
            case Task.SCAVENGE:
                selected = scavengeButton;
                break;
            case Task.DEFEND:
                selected = defendButton;
                break;
            default:
                throw new System.Exception( "Unhandled task" );
        }

        if( previousSelected )
        {
            ColorBlock previousSelectedColors = previousSelected.colors;
            previousSelectedColors.normalColor = Color.white;
            previousSelectedColors.highlightedColor = Color.gray;
            previousSelected.colors = previousSelectedColors;
        }

        ColorBlock selectedColors = selected.colors;
        selectedColors.normalColor = Color.gray;
        selectedColors.highlightedColor = Color.black;
        selected.colors = selectedColors;
    }

    public bool HasTask()
    {
        return task != Task.NONE;
    }

    public void TargetCharacter( Character character )
    {
        characterPortrait.sprite = character.traitsSprite;
        Character.Hunger hunger = character.GetHunger();
        Character.Hydration hydration = character.GetHydration();
        Character.Injury injury = character.GetInjury();
        Character.Sickness sickness = character.GetSickness();

        hungerImage.sprite = statusManager.hungerStateSprites[( int )hunger];
        thirstImage.sprite = statusManager.hydratedStateSprites[( int )hydration];
        injuryImage.sprite = statusManager.injuryStateSprites[( int )injury];
        sicknessImage.sprite = statusManager.sicknessStateSprites[( int )sickness];
    }
}
