using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Time_Based_Dynamic_Story
{
    public GameObject dynamicStory;
    public Timestamp timeWhenToDisplay;

    bool hasPlayed = false;

    public bool HasPlayed()
    {
        return hasPlayed;
    }

    public void Played()
    {
        hasPlayed = true;
    }
}

public class Story_Manager : MonoBehaviour
{
    public List<Day_Story_Element> dayStories;
    public List<Time_Based_Dynamic_Story> timeBasedDynamicStories;

    Queue<GameObject> dynamicStories = new Queue<GameObject>();
    

    Time_Manager timeManager;

    void Awake()
    {
        timeManager = GameObject.Find( "Time_Manager" ).GetComponent<Time_Manager>();
    }

    void Update()
    {
        Timestamp currentTime = timeManager.GetTime();

        foreach( Time_Based_Dynamic_Story dynamicStory in timeBasedDynamicStories )
        {
            if( dynamicStory.HasPlayed() )
            {
                continue;
            }

            if( currentTime.day > dynamicStory.timeWhenToDisplay.day )
            {
                dynamicStories.Enqueue( dynamicStory.dynamicStory );
                dynamicStory.Played();
            }
            else if( currentTime.day == dynamicStory.timeWhenToDisplay.day && currentTime.hour >= dynamicStory.timeWhenToDisplay.hour )
            {
                dynamicStories.Enqueue( dynamicStory.dynamicStory );
                dynamicStory.Played();
            }
            else if( currentTime.day == dynamicStory.timeWhenToDisplay.day && currentTime.hour == dynamicStory.timeWhenToDisplay.hour && currentTime.minute >= dynamicStory.timeWhenToDisplay.minute )
            {
                dynamicStories.Enqueue( dynamicStory.dynamicStory );
                dynamicStory.Played();
            }
        }
    }

    public bool DayStoryAvailable()
    {
        Timestamp currentTime = timeManager.GetTime();

        return dayStories.Count >= currentTime.day;
    }

    public Day_Story_Element GetDayStory()
    {
        Timestamp currentTime = timeManager.GetTime();

        return dayStories[(int)currentTime.day - 1];
    }

    public bool DynamicStoryAvailable()
    {
        return dynamicStories.Count > 0;
    }

    public GameObject GetDynamicStory()
    {
        return dynamicStories.Dequeue();
    }
}
