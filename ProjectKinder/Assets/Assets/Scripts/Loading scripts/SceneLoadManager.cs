using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoadManagerAsync : MonoBehaviour {

    public static SceneLoadManagerAsync Instance { get; set; }

    public string[] ScenesToLoadOnAwake = new string[0];

    private void Awake()
    {
        Application.backgroundLoadingPriority = ThreadPriority.Normal;
        Instance = this;

        foreach (var scene in ScenesToLoadOnAwake)
        {
            Load(scene);
        }
    }

    // Load a scene
    public void Load(string sceneName)
    {
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

        }
    }

    public void Unload(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.UnloadScene(sceneName);
        }
    }


}
