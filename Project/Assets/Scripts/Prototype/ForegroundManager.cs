// RATIONALE ------------------------------------
// * Manages the foreground image
// * Provides functionality for other systems to interact with the foregroundImage

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForegroundManager : MonoBehaviour
{
    public GameObject foreground;
    public Image foregroundImage;
    public float secondsToFadeIn = 1.0f;
    public float secondsToFadeOut = 1.0f;

    private float currentFadeInValue = 0f;
    private float currentFadeOutValue = 0f;

    public enum State
    {
        IN,
        FADING_IN,
        OUT,
        FADING_OUT
    }

    private State state = State.OUT;


	// Use this for initialization
	void Start ()
    {
	    state = State.OUT;	
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch( state )
        {
            case State.FADING_IN:
            {
                Color32 color = foregroundImage.color;
                float fadeInRate = Time.deltaTime / secondsToFadeIn;
                currentFadeInValue += Mathf.Lerp( 0f, 255f, fadeInRate );
    
                while( currentFadeInValue > 1.0f && color.a < 255 )
                {
                    color.a += 1;
                    currentFadeInValue -= 1.0f;
                }

                foregroundImage.color = color;

                if( color.a == 255 )
                {
                    state = State.IN;
                    currentFadeInValue = 0f;
                }
                
                break;
            }
            case State.FADING_OUT:
            {
                Color32 color = foregroundImage.color;
                float fadeOutRate = Time.deltaTime / secondsToFadeOut;
                currentFadeOutValue += Mathf.Lerp( 0f, 255f, fadeOutRate );
    
                while( currentFadeOutValue > 1.0f && color.a > 0 )
                {
                    color.a -= 1;
                    currentFadeOutValue -= 1.0f;
                }

                foregroundImage.color = color;

                if( color.a == 0 )
                {
                    state = State.OUT;
                    currentFadeOutValue = 0f;
                    foreground.SetActive( false );
                }

                break;
            }
        }		
	}

    public void FadeIn()
    {
        foreground.SetActive( true );

        if( state == State.OUT )
        {
            state = State.FADING_IN;
        }
    }

    public void FadeOut()
    {
        if( state == State.IN )
        {
            state = State.FADING_OUT;
        }
    }

    public State GetState()
    {
        return state;
    }
}
