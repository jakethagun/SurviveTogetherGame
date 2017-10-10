using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Required_Materials_To_Upgrade
{
    public List<Item_And_Quantity> requiredItems;
    public List<string> perksText;
}

public class Home_Manager : MonoBehaviour
{
    private int currentHouseLevel = 0;

    public List<Required_Materials_To_Upgrade> requiredMaterialsToUpgrade;
    Storage_Manager storageManager;

    // Use this for initialization
    void Start()
    {
        storageManager = GameObject.Find( "Storage_Manager" ).GetComponent<Storage_Manager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Upgrade()
    {
        foreach( Item_And_Quantity requiredItemAndQuantity in GetRequiredItemsToUpgrade() )
        {
            storageManager.SubtractQuantity( requiredItemAndQuantity );
        }

        ++currentHouseLevel;
    }

    public int CurrentLevel()
    {
        return currentHouseLevel;
    }

    public bool HasUpgradeAvailable()
    {
        return currentHouseLevel < requiredMaterialsToUpgrade.Count;
    }

    public List<Item_And_Quantity> GetRequiredItemsToUpgrade()
    {
        return requiredMaterialsToUpgrade[currentHouseLevel].requiredItems;
    }

    public List<string> GetPerksOfUpgrade()
    {
        return requiredMaterialsToUpgrade[currentHouseLevel].perksText;
    }
}
