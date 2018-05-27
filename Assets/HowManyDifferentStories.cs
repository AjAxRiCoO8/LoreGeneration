using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowManyDifferentStories : MonoBehaviour {

    public LoreManager manager;

    public int amountOfRuns;

    List<List<int>> allStories = new List<List<int>>();

    List<List<int>> allStoryEndStates = new List<List<int>>();

    List<List<int>> processedStories = new List<List<int>>();

    List<List<int>> processedEndings = new List<List<int>>();

    int differentStories = 0;
    int differentEndings = 0;

    int i = 0;

	// Use this for initialization
	void Start () {
        manager.userChoicePercentage = 0;

		for (int i = 0; i < amountOfRuns; i++)
        {
            manager.ResetStory();

            List<int> story = new List<int>();

            while(!manager.StoryComplete)
            {
                manager.UpdateRules();

                story.Add(manager.lastChosenRule);

              //  Debug.LogWarning("Rule: " + manager.lastChosenRule);
            }

            List<int> endState = new List<int>(manager.StoryState);
            endState.Sort();

            allStoryEndStates.Add(endState);

            allStories.Add(story);
        }

        Debug.LogError("Amount of Stories: " + allStories.Count);
	}
	
	// Update is called once per frame
	void Update () {
        if (i < allStories.Count)
        { 
            bool isUnique = true;
            bool isUniqueEnding = true;

            for (int j = 0; j < processedStories.Count; j++)
            {
                if (allStories[i].Count == processedStories[j].Count)
                {
                    Debug.LogWarning("Same Length story");
                    int k = 0;
                    bool theSame = true;
                    for (; k < allStories[i].Count; k++)
                    {
                        if (allStories[i][k].CompareTo(processedStories[j][k]) != 0)
                        {
                            //Debug.LogWarning("Current K = " + k + ", where length of list is " + allStories[i].Count);
                            if (k >= 4)
                            {
                               // Debug.LogWarning(i + ", " + k + ": first: " + allStories[i][k] + ", second: " + processedStories[j][k] + "isEqual? " + allStories[i][k].CompareTo(processedStories[j][k]));
                            }
                            theSame = false;
                            break;
                        }
                        else
                        {
                            Debug.LogWarning("Same rule at same place");
                        }
                    }

                    if (theSame)
                    {
                        isUnique = false;
                        break;
                    }
                }
                else
                {
                    Debug.LogWarning("different Length story");
                }

                //Debug.LogWarning("EndingState Length: " + allStoryEndStates[i].Count);
                // Check endings
                if (allStoryEndStates[i].Count == processedEndings[j].Count)
                {

                    int k = 0;
                    bool theSame = true;
                    for (; k < allStoryEndStates[i].Count; k++)
                    {
                        if (allStoryEndStates[i][k].CompareTo(processedEndings[j][k]) != 0)
                        {
                            theSame = false;
                            break;
                        }
                    }

                    if (theSame)
                    {
                        isUniqueEnding = false;
                        break;
                    }
                }
            }

            if (isUnique)
            {
                differentStories++;
                Debug.LogWarning("Unique Story");
            }
            else
            {
                Debug.LogWarning("Identical Story");
            }

            if (isUniqueEnding)
            {
                differentEndings++;
                Debug.LogWarning("Unique Ending");
            }
            else
            {
                Debug.LogWarning("Identical Ending");
            }

            processedStories.Add(allStories[i]);
            processedEndings.Add(allStoryEndStates[i]);

            i++;
        }

        if (i == allStories.Count)
        {
            Debug.LogError("Unique Stories: " + differentStories);
            Debug.LogError("Unique Endings: " + differentEndings);

        }
    }
}
