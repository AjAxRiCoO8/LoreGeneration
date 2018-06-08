using UnityEngine;
using System.Collections;

public class UnloadOnTrigger : MonoBehaviour {

    public Trigger trigger;
    public float yieldTime;

    public string[] scenesToUnload;

    private bool activated;

    private IEnumerator coroutine;

    public virtual void Update()
    {
        if (!trigger.IsClosed && !activated)
        {
            foreach (var scene in scenesToUnload)
            {
              coroutine = UnloadScene(scene);
                StartCoroutine(coroutine);  
            }

            activated = true;
        }
    }

    private IEnumerator UnloadScene(string scene)
    {
        yield return new WaitForSeconds(yieldTime);
        SceneLoadManager.Instance.Unload(scene);
    }
}
