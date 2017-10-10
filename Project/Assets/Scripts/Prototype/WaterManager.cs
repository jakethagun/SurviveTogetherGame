using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WaterState {
    HYDRATED,
    SLIGHTLY_THIRSTY,
    THIRSTY,
    VERY_THIRSTY
}

public class WaterManager : MonoBehaviour 
{
    // References -------------------------------
    public SurvivorsManager survivorsManager;

    // Water -------------------------------------
    public uint startWater = 0;
    public uint currentWater = 0;
    public uint waterGatheredForCharacter = 1;
    public uint waterGatheredPerSurvivor = 1;

    // GUI --------------------------------------
    public Text waterGUI;

	// Unity callbacks --------------------------
	void Start () 
    {
		currentWater = startWater;
	}
	
	void Update () 
    {
		
	}

    void OnGUI()
    {
        waterGUI.text = "Water : " + currentWater.ToString();
    }

    // Gather -----------------------------------
    public void OnGathered()
    {
        currentWater += waterGatheredForCharacter + survivorsManager.GetNumberOfSurvivors() * waterGatheredPerSurvivor;
    }

    public uint GetWater() {
        return currentWater;
    }

    public void ConsumeWater( uint amount )
    {
        currentWater -= amount;
    }
}
