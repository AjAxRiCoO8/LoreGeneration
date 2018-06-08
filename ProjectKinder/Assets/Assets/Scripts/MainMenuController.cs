using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MainMenuController : MonoBehaviour
{
    public void ChangeScene(string name)
    {
        if (name == "ExitGame")
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(name);
        }
    }
}
