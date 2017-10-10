using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Obtain_Survivor : MonoBehaviour
{
    bool random = false;
    public Character survivor;

    Survivor_Manager survivorManager;

    private void Awake()
    {
        survivorManager = GameObject.Find( "Survivor_Manager" ).GetComponent<Survivor_Manager>();

        if( !random )
        {
            survivorManager.AddSurvivor( survivor );
        }
        else
        {
            survivorManager.AddRandomSurvivor();
        }
    }
}
