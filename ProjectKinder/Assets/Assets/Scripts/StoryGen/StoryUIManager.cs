using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryUIManager : MonoBehaviour
{
    [SerializeField]
    public Text storyTextUI;

    [HideInInspector]
    static StoryUIManager instance;

    private void Awake()
    {
        instance = this;
    }

    public static StoryUIManager GetInstance()
    {
        return instance;
    }

    public void SetStoryText(string text)
    {
        string test = text;
        storyTextUI.text = test;
    }
}
