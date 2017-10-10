using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Farm_Manager : MonoBehaviour
{
    public uint currentFarmFood = 0;
    int currentUpgradeLevel;

    public float farmSystemQuantity = 0f;
    public float farmSystemMaximumQuantity = 100f;
    public float farmSystemRatePerHour = 10f;
    Timestamp farmSystemUpdateTime;

    public uint foodGatheredForEachSurvivor = 1;

    //public List<Water_Gathered_Per_Level> waterGatheredAtEachUpgrade = new List<Water_Gathered_Per_Level>();

    Time_Manager timeManager;

    void Awake()
    {
        timeManager = GameObject.Find( "Time_Manager" ).GetComponent<Time_Manager>();
        farmSystemUpdateTime = timeManager.StartGameTime();
    }

    public uint CurrentFarmFood()
    {
        return currentFarmFood;
    }

    public void HarvestFarm()
    {
        if( farmSystemQuantity != farmSystemMaximumQuantity )
        {
            throw new System.Exception( "Expected food bar to be full" );
        }

        Survivor_Manager survivorManager = GameObject.Find( "Survivor_Manager" ).GetComponent<Survivor_Manager>();
        currentFarmFood += foodGatheredForEachSurvivor * ( 1 + survivorManager.NumberOfSurvivors() );

        farmSystemQuantity = 0f;
    }

    public float GetFarmSystemQuantity()
    {
        uint minutesSinceLastUpdated = timeManager.MinutesBetweenTimestamps( farmSystemUpdateTime, timeManager.GetTime() );

        if( minutesSinceLastUpdated > 0 )
        {
            farmSystemQuantity += minutesSinceLastUpdated * GetFarmSystemRatePerHour() / 60f;
            farmSystemUpdateTime = timeManager.GetTime();

            if( farmSystemQuantity > GetMaximumFarmSystemQuantity() )
            {
                farmSystemQuantity = GetMaximumFarmSystemQuantity();
            }
        }

        return farmSystemQuantity;
    }

    public float GetFarmSystemRatePerHour()
    {
        return farmSystemRatePerHour;
    }

    public float GetMaximumFarmSystemQuantity()
    {
        return farmSystemMaximumQuantity;
    }
}
