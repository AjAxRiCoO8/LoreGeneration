using UnityEngine;
using System.Collections;

public class TreasureRoomTrigger : MonoBehaviour {

    public MusicController musicController;
    public MusicController.GameMusic gameMusic;

    public GameObject trigger;
    public GameObject[] mummy;

    private bool Activated;

	// Update is called once per frame
	void Update () {
        if (GetComponent<TreasureRoomDoorsScript>().open && !Activated)
        {
            Activated = true;
            StartEffects();
        }
	}

    void StartEffects()
    {
        IEnumerator changeMusic = ChangeMusic(0);
        StartCoroutine(changeMusic);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            player.GetComponentInChildren<CameraShake>().shake = true;
        }
        
        trigger.GetComponent<PillarTrigger>().activate = true;
        for (int i = 0; i < mummy.Length; i++)
        {
            mummy[i].GetComponent<MummySleeping>().WakeUp();
            mummy[i].GetComponent<MummyVariables>().LoadInPlayerData();
        }
}

    private IEnumerator ChangeMusic(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        musicController.ChangeMusic(gameMusic);
    }
}
