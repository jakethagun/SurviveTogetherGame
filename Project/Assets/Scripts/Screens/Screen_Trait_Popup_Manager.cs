using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen_Trait_Popup_Manager : MonoBehaviour {

    private void Awake()
    {
        Transform canvas = GameObject.Find( "Canvas" ).GetComponent<Transform>();
        transform.SetParent( canvas );
    }

    // Use this for initialization
    void Update ()
    {
        if( Input.GetMouseButtonDown( 0 ) )
            GameObject.Destroy( gameObject );
	}
	
}
