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

    bool initialized = false;
	
	// Update is called once per frame
	void Update () {

        if (!initialized)
        {
            if (i < amountOfRuns)
            {
                manager.ResetStory();

                List<int> story = new List<int>();

                while (!manager.StoryComplete)
                {
                    manager.UpdateRules();

                    story.Add(manager.lastChosenRule);

                    //  Debug.LogWarning("Rule: " + manager.lastChosenRule);
                }

                List<int> endState = new List<int>(manager.StoryState);
                endState.Sort();

                allStoryEndStates.Add(endState);

                allStories.Add(story);

                Debug.LogWarning("Story run");

                i++;
            }

            if (i == amountOfRuns)
            {
                i = 0;
                Debug.LogError("Amount of Stories: " + allStories.Count);
                initialized = true;
            }
        }
        else
        {

            if (i < allStories.Count)
            {
                bool isUnique = true;
                bool isUniqueEnding = true;

                for (int j = 0; j < processedStories.Count; j++)
                {
                    if (allStories[i].Count == processedStories[j].Count)
                    {
                        int k = 0;
                        bool theSame = true;
                        for (; k < allStories[i].Count; k++)
                        {
                            if (allStories[i][k].CompareTo(processedStories[j][k]) != 0)
                            {
                                theSame = false;
                                break;
                            }
                        }

                        if (theSame)
                        {
                            isUnique = false;
                            break;
                        }
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
}
