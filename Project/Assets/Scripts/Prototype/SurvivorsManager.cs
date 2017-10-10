// RATIONALE ------------------------------------
// * Manages survivor related GUI
// * Manages survivors

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurvivorsManager : MonoBehaviour
{
    // GUI
    public Text survivorsText;

    public uint startingNumberOfSurvivors = 0;
    public uint numberOfSurvivors = 0;

	// Unity Callbacks
	void Start () 
    {
		numberOfSurvivors = startingNumberOfSurvivors;
	}
	
	void Update () 
    {
		
	}

    void OnGUI()
    {
        survivorsText.text = "Survivors: " + numberOfSurvivors.ToString();
    }

    public uint GetNumberOfSurvivors()
    {
        return numberOfSurvivors;
    }

    public void KillRandom()
    {
        --numberOfSurvivors;
    }
}
