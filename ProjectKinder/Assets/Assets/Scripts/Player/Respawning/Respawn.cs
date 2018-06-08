using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour
{
    public GameObject respawnPoint;

    public void RespawnAtPoint()
    {
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.transform.position;
        }
    }

    public void SetCurrentSpawnPoint(GameObject respawnPoint)
    {
        this.respawnPoint = respawnPoint;
    }
}
