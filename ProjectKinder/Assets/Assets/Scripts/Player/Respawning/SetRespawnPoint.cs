using UnityEngine;
using System.Collections;

public class SetRespawnPoint : MonoBehaviour
{
    public GameObject[] players;
    public GameObject effects;

    private void Start()
    {
        StartCoroutine(WaitForPlayers(2));
    }

    private void OnTriggerEnter(Collider collider)
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        if (collider.CompareTag("Player"))
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<Respawn>().SetCurrentSpawnPoint(transform.Find("Spawn Point " + (i + 1)).gameObject);
                effects.gameObject.SetActive(true);
            }
        }
    }

    private IEnumerator WaitForPlayers(float time)
    {
        yield return new WaitForSeconds(time);

        players = GameObject.FindGameObjectsWithTag("Player");
    }
}
