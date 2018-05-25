using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text storyTextUI;

    [SerializeField]
    public GameObject choiceUI;

    [SerializeField]
    public Button[] storyOptionButtonsUI;

    [SerializeField]
    public GameObject continueUI;

    [HideInInspector]
    static UIManager instance;


    private void Start()
    {
        instance = this;
    }

    public void ActivateChoiceUI()
    {
        choiceUI.SetActive(true);
        continueUI.SetActive(false);
    }

    public void DeactivateChoiceUI()
    {
        choiceUI.SetActive(false);
        continueUI.SetActive(true);

        foreach (var button in storyOptionButtonsUI)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    public static UIManager GetInstance()
    {
        return instance;
    }

    public void SetStoryText(string text)
    {
        storyTextUI.text = text;
    }
}
