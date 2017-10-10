using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Default_Characters_Manager : MonoBehaviour
{
    public List< Character > characters = new List< Character >();

    // Unity Callbacks --------------------------
    void Awake()
    {
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public List< Character > GetDefaultCharacters()
    {
        return characters;
    }

    public Character GetDefaultCharacter( string name )
    {
        foreach( var character in characters )
        {
            if( character.name == name )
            {
                return character;
            }
        }

        return null;
    }
}
