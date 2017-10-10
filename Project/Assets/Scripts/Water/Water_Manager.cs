using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Water_Gathered_Per_Level
{
    public float waterGatheredPerHour;
    public float maximumWater;
}

public class Water_Manager : MonoBehaviour
{
    float currentWater;
    int currentUpgradeLevel;

    public float waterSystemQuantity = 0f;
    public float waterSystemMaximumQuantity = 100f;
    public float waterSystemRatePerHour = 10f;
    Timestamp waterSystemUpdateTime;

    public float waterGatheredForEachSurvivor = 1.0f;

    //public List<Water_Gathered_Per_Level> waterGatheredAtEachUpgrade = new List<Water_Gathered_Per_Level>();

    Time_Manager timeManager;

    void Awake()
    {
        timeManager = GameObject.Find( "Time_Manager" ).GetComponent<Time_Manager>();
        waterSystemUpdateTime = timeManager.StartGameTime();
    }

    public void HarvestWater()
    {
        if( waterSystemQuantity != waterSystemMaximumQuantity )
        {
            throw new System.Exception( "Expected water bar to be full" );
        }

        Survivor_Manager survivorManager = GameObject.Find( "Survivor_Manager" ).GetComponent<Survivor_Manager>();
        currentWater += waterGatheredForEachSurvivor * ( 1 + survivorManager.NumberOfSurvivors() );

        waterSystemQuantity = 0f;
    }

    public float GetCurrentWater()
    {
        return currentWater;
    }

    public float GetWaterSystemQuantity()
    {
        uint minutesSinceLastUpdated = timeManager.MinutesBetweenTimestamps( waterSystemUpdateTime, timeManager.GetTime() );

        if( minutesSinceLastUpdated > 0 )
        {
            waterSystemQuantity += minutesSinceLastUpdated * GetWaterSystemRatePerHour() / 60f;
            waterSystemUpdateTime = timeManager.GetTime();

            if( waterSystemQuantity > GetMaximumWaterSystemQuantity() )
            {
                waterSystemQuantity = GetMaximumWaterSystemQuantity();
            }
        }

        return waterSystemQuantity;
    }

    public float GetWaterSystemMaximum()
    {
        return waterSystemMaximumQuantity;
    }

    public float GetWaterSystemRatePerHour()
    {
        return waterSystemRatePerHour;
    }

    public float GetMaximumWaterSystemQuantity()
    {
        return waterSystemMaximumQuantity;
    }
}
