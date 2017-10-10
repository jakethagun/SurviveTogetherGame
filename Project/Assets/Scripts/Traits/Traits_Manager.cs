using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traits_Manager : MonoBehaviour {

    public uint traitPoints = 0;

    public List<Sprite> traitsSprites = new List<Sprite>();
    public List<GameObject> traitsPopupScreens = new List<GameObject>();
    public List<GameObject> traitUpgradePopupScreen = new List<GameObject>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public uint GetTraitPoints()
    {
        return traitPoints;
    }
}
