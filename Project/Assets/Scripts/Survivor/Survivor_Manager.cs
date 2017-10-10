using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survivor_Manager : MonoBehaviour
{
    List<Character> survivors = new List<Character>();

    public void AddSurvivor( Character survivor )
    {
        GameObject newSurvivor = GameObject.Instantiate( survivor.gameObject );

        survivors.Add( newSurvivor.GetComponent<Character>() );
    }

    public void AddRandomSurvivor()
    {
        List<Character> characters = GameObject.Find( "Default_Characters_Manager" ).GetComponent<Default_Characters_Manager>().GetDefaultCharacters();
        List<Character> survivorsDontHave = new List<Character>();

        foreach( Character character in characters )
        {
            bool hasSurvivor = false;

            foreach( Character survivor in survivors )
            {
                if( character.name == survivor.name )
                {
                    hasSurvivor = true;
                    break;
                }
            }

            if( !hasSurvivor )
            {
                survivorsDontHave.Add( character );
            }
        }

        int randomIndex = Random.Range( 0, survivorsDontHave.Count );

        AddSurvivor( survivorsDontHave[randomIndex] );
    }

    public uint NumberOfSurvivors()
    {
        return ( uint )survivors.Count;
    }
}
