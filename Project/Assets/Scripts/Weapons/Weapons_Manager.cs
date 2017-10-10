using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipped_Weapon
{
    public Item_And_Quantity itemAndQuantity;
    public int equipped;

    public Equipped_Weapon( Item_And_Quantity inItemAndQuantity )
    {
        itemAndQuantity = inItemAndQuantity;
        equipped = 0;
    }
}

public class Weapons_Manager : MonoBehaviour
{
    Dictionary<string, Equipped_Weapon> weapons = new Dictionary<string, Equipped_Weapon>();

    Storage_Manager storageManager;

    private void Awake()
    {
        storageManager = GameObject.Find( "Storage_Manager" ).GetComponent<Storage_Manager>();
    }

    private void Update()
    {
        List<Item_And_Quantity> weaponItems = new List<Item_And_Quantity>();
        weaponItems = storageManager.GetStoredItems( Item.Category.WEAPONS, Item.Rarity.COMMON );
        weaponItems.AddRange( storageManager.GetStoredItems( Item.Category.WEAPONS, Item.Rarity.UNCOMMON ) );
        weaponItems.AddRange( storageManager.GetStoredItems( Item.Category.WEAPONS, Item.Rarity.RARE ) );

        foreach( Item_And_Quantity weaponItem in weaponItems )
        {
            if( weaponItem.quantity > 0 )
            {
                if( weapons.ContainsKey( weaponItem.item.name ) )
                {
                    weapons[weaponItem.item.name].itemAndQuantity = weaponItem;
                }
                else
                {
                    weapons.Add( weaponItem.item.name, new Equipped_Weapon( weaponItem ) );
                }
            }
        }
    }

    public List<Equipped_Weapon> Weapons()
    {
        return new List<Equipped_Weapon>( weapons.Values );
    }


    public void Equip( Item item )
    {
        weapons[item.name].equipped += 1;
    }

    public void Unequip( Item item )
    {
        weapons[item.name].equipped -= 1;
    }
    
}
