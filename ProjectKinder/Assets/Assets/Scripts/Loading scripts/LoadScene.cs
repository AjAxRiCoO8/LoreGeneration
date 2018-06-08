using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {

    public string[] scenesToLoad;

    // Use this for initialization
    public void LoadSceneAdditive () {

        foreach (var scene in scenesToLoad)
        {
            SceneLoadManager.Instance.Load(scene);
        }

    }
}
