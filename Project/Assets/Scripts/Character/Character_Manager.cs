using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Manager : MonoBehaviour 
{
    private Character character;



	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Character --------------------------------
    public void SetCharacter( string characterName )
    {
        Default_Characters_Manager defaultCharacters = GameObject.Find( "Default_Characters_Manager" ).GetComponent< Default_Characters_Manager >();
        Character defaultCharacter = defaultCharacters.GetDefaultCharacter( characterName );
        GameObject characterGO = GameObject.Instantiate( defaultCharacter.gameObject );
        characterGO.name = defaultCharacter.name;
        character = characterGO.GetComponent< Character >();
    } 

    public Character GetCharacter()
    {
        return character;
    }
}
