using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodManager : MonoBehaviour 
{
    // References -------------------------------
    public SurvivorsManager survivorsManager;

    // Food -------------------------------------
    public uint startFood = 0;
    public uint currentFood = 0;
    public uint foodGatheredForCharacter = 1;
    public uint foodGatheredPerSurvivor = 1;

    // GUI --------------------------------------
    public Text foodGUI;

	// Unity callbacks --------------------------
	void Start () 
    {
		currentFood = startFood;
	}
	
	void Update () 
    {
		
	}

    void OnGUI()
    {
        foodGUI.text = "Food : " + currentFood.ToString();
    }

    // Gather -----------------------------------
    public void OnGathered()
    {
        currentFood += foodGatheredForCharacter + survivorsManager.GetNumberOfSurvivors() * foodGatheredPerSurvivor;
    }

     public uint GetFood() {
        return currentFood;
    }

    public void ConsumeFood( uint amount )
    {
        currentFood -= amount;
    }
}
