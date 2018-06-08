using UnityEngine;
using System.Collections;

public class LoadOnTrigger : MonoBehaviour {

    public DoorDownwards trigger;

    public string[] scenesToLoad;

    private bool activated;

    private IEnumerator coroutine;

    public virtual void Update()
    {
        if (!trigger.IsClosed && !activated)
        {
            foreach (var scene in scenesToLoad)
            {
                coroutine = LoadScene(scene);
                StartCoroutine(coroutine);
            }

            activated = true;
        }
    }

    private IEnumerator LoadScene(string scene)
    {
        yield return new WaitForSeconds(0);
        SceneLoadManager.Instance.Load(scene);
    }
}
